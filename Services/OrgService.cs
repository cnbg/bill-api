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
            .Include(o => o.OrgType)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<OrgDto> GetOrgByIdAsync(Guid id)
    {
        var org = await dbCtx.Orgs
            .Include(o => o.OrgType)
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new OrgDto(
                r.Id,
                r.OrgTypeId,
                r.Name,
                r.Inn,
                r.Okpo,
                r.Balance,
                r.Note,
                r.IsActive
            ))
            .FirstOrDefaultAsync();

        return org ?? throw new KeyNotFoundException("Org not found");
    }

    public async Task<OrgDto> CreateOrgAsync(CreateOrgRequest request)
    {
        var resp = dbCtx.Orgs.Add(new Org
        {
            OrgTypeId = request.OrgTypeId,
            Name = request.Name,
            Inn = request.Inn,
            Okpo = request.Okpo,
            Balance = request.Balance,
            Note = request.Note,
            IsActive = request.IsActive ?? false
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new OrgDto(
                resp.Entity.Id,
                resp.Entity.OrgTypeId,
                resp.Entity.Name,
                resp.Entity.Inn,
                resp.Entity.Okpo,
                resp.Entity.Balance,
                resp.Entity.Note,
                resp.Entity.IsActive
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

        org.OrgTypeId = request.OrgTypeId ?? org.OrgTypeId;
        org.Name = request.Name ?? org.Name;
        org.Inn = request.Inn ?? org.Inn;
        org.Okpo = request.Okpo ?? org.Okpo;
        org.Balance = request.Balance ?? org.Balance;
        org.Note = request.Note ?? org.Note;
        org.IsActive = request.IsActive ?? org.IsActive;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteOrgAsync(Guid id)
    {
        var org = await dbCtx.Orgs
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (org == null)
            throw new KeyNotFoundException("Org not found");

        dbCtx.Orgs.Remove(org);
        await dbCtx.SaveChangesAsync();
    }
}
