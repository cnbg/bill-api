using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class DistrictService(AppDbCtx dbCtx) : IDistrictService
{
    public async Task<LoadResult> GetDistrictListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Districts
            .Include(r => r.Region)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<DistrictResponse> GetDistrictByIdAsync(Guid id)
    {
        var district = await dbCtx.Districts
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new DistrictResponse(
                r.Id,
                r.Code,
                r.Name
            ))
            .FirstOrDefaultAsync();

        return district ?? throw new KeyNotFoundException("District not found");
    }

    public async Task<DistrictResponse> CreateDistrictAsync(CreateDistrictRequest request)
    {
        // Check for duplicate code
        var existingDistrict = await dbCtx.Districts
            .Where(r => r.Code == request.Code)
            .SingleOrDefaultAsync();

        if (existingDistrict != null)
            throw new ArgumentException("District with the same code already exists");

        var resp = dbCtx.Districts.Add(new District
        {
            Code = request.Code,
            Name = request.Name,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new DistrictResponse(
                resp.Entity.Id,
                resp.Entity.Code,
                resp.Entity.Name
            )
            : throw new ArgumentException("Failed to create district");
    }

    public async Task UpdateDistrictAsync(Guid id, UpdateDistrictRequest request)
    {
        var district = await dbCtx.Districts
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (district == null)
            throw new KeyNotFoundException("District not found");

        // Check for duplicate code
        if (!string.IsNullOrEmpty(request.Code))
        {
            var existingDistrict = await dbCtx.Districts
                .Where(r => r.Code == request.Code && r.Id != id)
                .SingleOrDefaultAsync();

            if (existingDistrict != null)
                throw new ArgumentException("District with the same code already exists");
        }

        district.Code = request.Code ?? district.Code;
        district.Name = request.Name ?? district.Name;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteDistrictAsync(Guid id)
    {
        var district = await dbCtx.Districts
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (district == null)
            throw new KeyNotFoundException("District not found");

        dbCtx.Districts.Remove(district);
        await dbCtx.SaveChangesAsync();
    }
}
