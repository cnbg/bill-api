using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class OrgTypeService(AppDbCtx dbCtx) : IOrgTypeService
{
    public async Task<LoadResult> GetOrgTypeListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.OrgTypes
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<OrgTypeDto> GetOrgTypeByIdAsync(Guid id)
    {
        var orgType = await dbCtx.OrgTypes
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new OrgTypeDto(
                r.Id,
                r.Name
            ))
            .FirstOrDefaultAsync();

        return orgType ?? throw new KeyNotFoundException("OrgType not found");
    }

    public async Task<OrgTypeDto> CreateOrgTypeAsync(CreateOrgTypeRequest request)
    {
        var resp = dbCtx.OrgTypes.Add(new OrgType
        {
            Name = request.Name,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new OrgTypeDto(
                resp.Entity.Id,
                resp.Entity.Name
            )
            : throw new ArgumentException("Failed to create orgType");
    }

    public async Task UpdateOrgTypeAsync(Guid id, UpdateOrgTypeRequest request)
    {
        var orgType = await dbCtx.OrgTypes
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (orgType == null)
            throw new KeyNotFoundException("OrgType not found");

        orgType.Name = request.Name ?? orgType.Name;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteOrgTypeAsync(Guid id)
    {
        var orgType = await dbCtx.OrgTypes
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (orgType == null)
            throw new KeyNotFoundException("OrgType not found");

        dbCtx.OrgTypes.Remove(orgType);
        await dbCtx.SaveChangesAsync();
    }
}
