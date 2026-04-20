using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class RateService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : BaseAuthService(ctxAccessor), IRateService
{
    public async Task<LoadResult> GetRateListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Rates
            .Where(r => r.OrgId == JwtDto.OrgId)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<RateDto> GetRateByIdAsync(Guid id)
    {
        var rate = await dbCtx.Rates
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RateDto(
                r.Id,
                r.Name,
                r.Price,
                r.Type,
                r.Note,
                r.StartDate,
                r.EndDate,
                r.IsActive
            ))
            .FirstOrDefaultAsync();

        return rate ?? throw new KeyNotFoundException("Rate not found");
    }

    public async Task<RateDto> CreateRateAsync(CreateRateRequest request)
    {
        var resp = dbCtx.Rates.Add(new Rate
        {
            OrgId = JwtDto.OrgId,
            Name = request.Name,
            Price = request.Price,
            Type = request.Type,
            Note = request.Note,
            StartDate = DateOnly.Parse(request.StartDate),
            EndDate = string.IsNullOrWhiteSpace(request.EndDate) ? null : DateOnly.Parse(request.EndDate),
            IsActive = request.IsActive ?? false
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new RateDto(
                resp.Entity.Id,
                resp.Entity.Name,
                resp.Entity.Price,
                resp.Entity.Type,
                resp.Entity.Note,
                resp.Entity.StartDate,
                resp.Entity.EndDate,
                resp.Entity.IsActive
            )
            : throw new ArgumentException("Failed to create rate");
    }

    public async Task UpdateRateAsync(Guid id, UpdateRateRequest request)
    {
        var rate = await dbCtx.Rates
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (rate == null)
            throw new KeyNotFoundException("Rate not found");

        rate.Name = request.Name ?? rate.Name;
        rate.Type = request.Type ?? rate.Type;
        rate.Note = request.Note ?? rate.Note;
        rate.Price = request.Price ?? rate.Price;
        rate.StartDate = request.StartDate != null ? DateOnly.Parse(request.StartDate) : rate.StartDate;
        rate.EndDate = request.EndDate != null ? DateOnly.Parse(request.EndDate) : rate.EndDate;
        rate.IsActive = request.IsActive ?? rate.IsActive;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteRateAsync(Guid id)
    {
        var rate = await dbCtx.Rates
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (rate == null)
            throw new KeyNotFoundException("Rate not found");

        dbCtx.Rates.Remove(rate);
        await dbCtx.SaveChangesAsync();
    }
}
