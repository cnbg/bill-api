using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using billing;
using billing.APIs;
using billing.Constants;
using billing.Helpers;
using billing.Providers;
using billing.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// AddDbContext with connection resiliency and pool for better performance and reliability
builder.Services.AddDbContextPool<AppDbCtx>(options =>
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            })
        .UseSnakeCaseNamingConvention()
);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.Events = new JwtBearerEvents
        // {
        //     OnMessageReceived = context =>
        //     {
        //         // Read JWT from cookie
        //         var token = context.Request.Cookies["access_token"];
        //         if (!string.IsNullOrEmpty(token)) context.Token = token;
        //
        //         return Task.CompletedTask;
        //     }
        // };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = conf["Jwt:Issuer"]!,
            ValidateAudience = true,
            ValidAudience = conf["Jwt:Audience"]!,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(conf.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("auth", config =>
    {
        config.Window = TimeSpan.FromMinutes(1);
        config.PermitLimit = conf.GetSection("Api:AuthRateLimit").Get<int>();
    });

    options.AddFixedWindowLimiter("api", config =>
    {
        config.Window = TimeSpan.FromMinutes(1);
        config.PermitLimit = conf.GetSection("Api:RateLimit").Get<int>();
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("perm", policy => policy.RequireAuthenticatedUser());

builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddProblemDetails();

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/json"]);
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IOrgTypeService, OrgTypeService>();
builder.Services.AddScoped<IOrgService, OrgService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IClientTypeService, ClientTypeService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IChargeService, ChargeService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddOpenApi();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseStatusCodePages();

app.MapAuthEndpoints().RequireRateLimiting("auth");
app.MapRegionEndpoints().RequireRateLimiting("api");
app.MapDistrictEndpoints().RequireRateLimiting("api");
app.MapOrgTypeEndpoints().RequireRateLimiting("api");
app.MapOrgEndpoints().RequireRateLimiting("api");
app.MapPermEndpoints().RequireRateLimiting("api");
app.MapRoleEndpoints().RequireRateLimiting("api");
app.MapClientTypeEndpoints().RequireRateLimiting("api");
app.MapClientEndpoints().RequireRateLimiting("api");
app.MapPaymentEndpoints().RequireRateLimiting("api");
app.MapChargeEndpoints().RequireRateLimiting("api");

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow })).WithTags("Health");

await app.RunAsync();
