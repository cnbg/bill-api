using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class UserService(AppDbCtx dbCtx) : IUserService
{
    public async Task<LoadResult> GetUserListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Users
            .Include(u => u.Org)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await dbCtx.Users
            .Include(u => u.Org)
            .AsNoTracking()
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDto(
                u.Id,
                u.OrgId,
                u.Org == null ? null : new OrgDto(u.Org.Id, u.Org.Name),
                u.FirstName,
                u.LastName,
                u.Email,
                u.Type
            ))
            .FirstOrDefaultAsync();

        return user ?? throw new KeyNotFoundException("User not found");
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        // Check for duplicate code
        var existingUser = await dbCtx.Users
            .Where(r => r.Email == request.Email)
            .SingleOrDefaultAsync();

        if (existingUser != null)
            throw new ArgumentException("User with the same code already exists");

        var resp = dbCtx.Users.Add(new User
        {
            OrgId = request.OrgId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new UserDto(
                resp.Entity.Id,
                resp.Entity.OrgId,
                null,
                resp.Entity.FirstName,
                resp.Entity.LastName,
                resp.Entity.Email,
                resp.Entity.Type
            )
            : throw new ArgumentException("Failed to create user");
    }

    public async Task UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var user = await dbCtx.Users
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException("User not found");

        // Check for duplicate code
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingUser = await dbCtx.Users
                .Where(r => r.Email == request.Email && r.Id != id)
                .SingleOrDefaultAsync();

            if (existingUser != null)
                throw new ArgumentException("User with the same code already exists");
        }

        user.OrgId = request.OrgId ?? user.OrgId;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await dbCtx.Users
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException("User not found");

        dbCtx.Users.Remove(user);
        await dbCtx.SaveChangesAsync();
    }
}
