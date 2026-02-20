using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class OrgService(AppDbCtx dbCtx) : IOrgService
{
    public async Task<LoadResult> GetOrgListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Orgs
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<OrgDto> GetOrgByIdAsync(Guid id)
    {
        var org = await dbCtx.Orgs
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new OrgDto(
                r.Id,
                r.Name
            ))
            .FirstOrDefaultAsync();

        return org ?? throw new KeyNotFoundException("Org not found");
    }

    public async Task<OrgDto> CreateOrgAsync(CreateOrgRequest request)
    {
        var resp = dbCtx.Orgs.Add(new Org
        {
            Name = request.Name,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new OrgDto(
                resp.Entity.Id,
                resp.Entity.Name
            )
            : throw new ArgumentException("Failed to create org");
    }

    public async Task UpdateOrgAsync(Guid id, UpdateOrgRequest request)
    {
        var org = await dbCtx.Orgs
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (org == null)
            throw new KeyNotFoundException("Org not found");

        org.Name = request.Name ?? org.Name;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteOrgAsync(IEnumerable<Guid> ids)
    {
        var org = await dbCtx.Orgs
            .Where(r => ids.Contains(r.Id))
            .FirstOrDefaultAsync();

        if (org == null)
            throw new KeyNotFoundException("Org not found");

        dbCtx.Orgs.Remove(org);

        await dbCtx.SaveChangesAsync();
    }
}
