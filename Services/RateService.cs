using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class RateService(AppDbCtx dbCtx) : IRateService
{
    public async Task<LoadResult> GetRateListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Rates
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
                r.Name
            ))
            .FirstOrDefaultAsync();

        return rate ?? throw new KeyNotFoundException("Rate not found");
    }

    public async Task<RateDto> CreateRateAsync(CreateRateRequest request)
    {
        var resp = dbCtx.Rates.Add(new Rate
        {
            Name = request.Name,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new RateDto(
                resp.Entity.Id,
                resp.Entity.Name
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
