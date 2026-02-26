using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class ClientService(AppDbCtx dbCtx) : IClientService
{
    public async Task<LoadResult> GetClientListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Clients
            .Include(c => c.ClientType)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<ClientDto> GetClientByIdAsync(Guid id)
    {
        var client = await dbCtx.Clients
            .Include(c => c.ClientType)
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ClientDto(
                c.Id,
                c.Account,
                c.ClientType.Name,
                c.Entrance,
                c.Floor,
                c.HouseNum,
                c.ApartNum,
                c.Area,
                c.MembersCount,
                c.Address,
                c.Note
            ))
            .FirstOrDefaultAsync();

        return client ?? throw new KeyNotFoundException("Client not found");
    }

    public async Task<ClientResponse> CreateClientAsync(CreateClientRequest request)
    {
        // Check for duplicate code
        var existingClient = await dbCtx.Clients
            .Where(r => r.Account == request.Account)
            .SingleOrDefaultAsync();

        if (existingClient != null)
            throw new ArgumentException("Client with the same code already exists");

        var resp = dbCtx.Clients.Add(new Client
        {
            Account = request.Account,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new ClientResponse(
                resp.Entity.Id,
                resp.Entity.Account
            )
            : throw new ArgumentException("Failed to create client");
    }

    public async Task UpdateClientAsync(Guid id, UpdateClientRequest request)
    {
        var client = await dbCtx.Clients
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        // Check for duplicate code
        if (!string.IsNullOrEmpty(request.Account))
        {
            var existingClient = await dbCtx.Clients
                .Where(r => r.Account == request.Account && r.Id != id)
                .SingleOrDefaultAsync();

            if (existingClient != null)
                throw new ArgumentException("Client with the same code already exists");
        }

        client.Account = request.Account ?? client.Account;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(Guid id)
    {
        var client = await dbCtx.Clients
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        dbCtx.Clients.Remove(client);
        await dbCtx.SaveChangesAsync();
    }
}
