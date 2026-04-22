using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class ClientTypeService(AppDbCtx dbCtx) : IClientTypeService
{
    public async Task<LoadResult> GetClientTypeListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.ClientTypes
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<ClientTypeDto> GetClientTypeByIdAsync(Guid id)
    {
        var clientType = await dbCtx.ClientTypes
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ClientTypeDto(
                r.Id,
                r.Name,
                r.IsActive
            ))
            .FirstOrDefaultAsync();

        return clientType ?? throw new KeyNotFoundException("ClientType not found");
    }

    public async Task<ClientTypeDto> CreateClientTypeAsync(CreateClientTypeRequest request)
    {
        var resp = dbCtx.ClientTypes.Add(new ClientType
        {
            Name = request.Name,
            IsActive = request.IsActive ?? false
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new ClientTypeDto(
                resp.Entity.Id,
                resp.Entity.Name,
                resp.Entity.IsActive
            )
            : throw new ArgumentException("Failed to create clientType");
    }

    public async Task UpdateClientTypeAsync(Guid id, UpdateClientTypeRequest request)
    {
        var clientType = await dbCtx.ClientTypes
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (clientType == null)
            throw new KeyNotFoundException("ClientType not found");

        clientType.Name = request.Name ?? clientType.Name;
        clientType.IsActive = request.IsActive ?? clientType.IsActive;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteClientTypeAsync(Guid id)
    {
        var clientType = await dbCtx.ClientTypes
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (clientType == null)
            throw new KeyNotFoundException("ClientType not found");

        dbCtx.ClientTypes.Remove(clientType);
        await dbCtx.SaveChangesAsync();
    }
}
