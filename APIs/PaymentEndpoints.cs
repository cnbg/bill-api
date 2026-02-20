using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/payment")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("payment");

        group.MapGet("list", async (
                IPaymentService paymentService,
                MyDataSourceLoadOptions loadOptions
            ) => await paymentService.GetPaymentListAsync(loadOptions))
            .RequirePermission(Permissions.PaymentView)
            .WithName("GetPaymentList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IPaymentService paymentService
            ) => await paymentService.GetPaymentByIdAsync(id))
            .RequirePermission(Permissions.PaymentView)
            .WithName("GetPaymentById");

        group.MapPost("create", async (
                IPaymentService paymentService,
                [FromBody] CreatePaymentRequest createPaymentDto
            ) => await paymentService.CreatePaymentAsync(createPaymentDto))
            .RequirePermission(Permissions.PaymentCreate)
            .WithValidation<CreatePaymentRequest>()
            .WithName("CreatePayment");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IPaymentService paymentService,
                [FromBody] UpdatePaymentRequest updatePaymentDto
            ) =>
            {
                await paymentService.UpdatePaymentAsync(id, updatePaymentDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.PaymentEdit)
            .WithValidation<UpdatePaymentRequest>()
            .WithName("UpdatePayment");

        group.MapDelete("delete", async (
                IPaymentService paymentService,
                [FromBody] IEnumerable<Guid> ids
            ) =>
            {
                await paymentService.DeletePaymentAsync(ids);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.PaymentDelete)
            .WithName("DeletePayment");

        return group;
    }
}
