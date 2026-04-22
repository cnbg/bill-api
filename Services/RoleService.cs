using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class RoleService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : BaseAuthService(ctxAccessor), IRoleService
{
    public async Task<LoadResult> GetRoleListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Roles
            .Where(r => r.OrgId == JwtDto.OrgId)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var role = await dbCtx.Roles
            .Where(r => r.Id == id && r.OrgId == JwtDto.OrgId)
            .Select(r => new RoleDto(
                r.Id,
                r.Name,
                r.IsActive,
                r.OrgId,
                r.Perms,
                r.Note
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return role ?? throw new KeyNotFoundException("Role not found");
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest request)
    {
        var resp = dbCtx.Roles.Add(new Role
        {
            OrgId = JwtDto.OrgId,
            Name = request.Name,
            IsActive = request.IsActive ?? false,
            Note = request.Note
        });

        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new RoleDto(
                resp.Entity.Id,
                resp.Entity.Name,
                resp.Entity.IsActive,
                resp.Entity.OrgId,
                resp.Entity.Perms,
                resp.Entity.Note
            )
            : throw new ArgumentException("Failed to create role");
    }

    public async Task UpdateRoleAsync(Guid id, UpdateRoleRequest request)
    {
        var role = await dbCtx.Roles
            .Where(r => r.Id == id && r.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (role == null)
            throw new KeyNotFoundException("Role not found");

        role.IsActive = request.IsActive ?? role.IsActive;
        role.Name = request.Name ?? role.Name;
        role.Note = request.Note ?? role.Note;

        await dbCtx.SaveChangesAsync();
    }

    public async Task UpdateRolePermAsync(Guid id, UpdateRolePermRequest request)
    {
        var role = await dbCtx.Roles
            .Where(r => r.Id == id && r.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (role == null)
            throw new KeyNotFoundException("Role not found");

        if (request.Add)
        {
            if (!role.Perms.Contains(request.Perm))
                role.Perms.Add(request.Perm);
        }
        else
        {
            role.Perms.Remove(request.Perm);
        }

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteRoleAsync(Guid id)
    {
        var role = await dbCtx.Roles
            .Where(r => r.Id == id && r.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (role == null)
            throw new KeyNotFoundException("Role not found");

        dbCtx.Roles.Remove(role);
        await dbCtx.SaveChangesAsync();
    }
}
