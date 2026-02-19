using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class RegionService(AppDbCtx dbCtx) : IRegionService
{
    public async Task<LoadResult> GetRegionListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Regions
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<RegionResponse> GetRegionByIdAsync(Guid id)
    {
        var region = await dbCtx.Regions
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RegionResponse(
                r.Id,
                r.Code,
                r.Name
            ))
            .FirstOrDefaultAsync();

        return region ?? throw new KeyNotFoundException("Region not found");
    }

    public async Task<RegionResponse> CreateRegionAsync(CreateRegionRequest request)
    {
        // Check for duplicate code
        var existingRegion = await dbCtx.Regions
            .Where(r => r.Code == request.Code)
            .SingleOrDefaultAsync();

        if (existingRegion != null)
            throw new ArgumentException("Region with the same code already exists");

        var resp = dbCtx.Regions.Add(new Region
        {
            Code = request.Code,
            Name = request.Name,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new RegionResponse(
                resp.Entity.Id,
                resp.Entity.Code,
                resp.Entity.Name
            )
            : throw new ArgumentException("Failed to create region");
    }

    public async Task UpdateRegionAsync(Guid id, UpdateRegionRequest request)
    {
        var region = await dbCtx.Regions
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (region == null)
            throw new KeyNotFoundException("Region not found");

        // Check for duplicate code
        if (!string.IsNullOrEmpty(request.Code))
        {
            var existingRegion = await dbCtx.Regions
                .Where(r => r.Code == request.Code && r.Id != id)
                .SingleOrDefaultAsync();

            if (existingRegion != null)
                throw new ArgumentException("Region with the same code already exists");
        }

        region.Code = request.Code ?? region.Code;
        region.Name = request.Name ?? region.Name;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteRegionAsync(Guid id)
    {
        var region = await dbCtx.Regions
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (region == null)
            throw new KeyNotFoundException("Region not found");

        dbCtx.Regions.Remove(region);
        await dbCtx.SaveChangesAsync();
    }
}
