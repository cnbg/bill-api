using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class BalanceService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : BaseAuthService(ctxAccessor), IBalanceService
{
    public async Task<LoadResult> GetBalanceListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Balances
            .Include(p => p.Org)
            .Where(p => p.OrgId == JwtDto.OrgId)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<BalanceDto> GetBalanceByIdAsync(Guid id)
    {
        var balance = await dbCtx.Balances
            .Include(p => p.Org)
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .Select(p => new BalanceDto(
                p.Id,
                p.Year,
                p.Month,
                p.Charges,
                p.Payments,
                p.Fines,
                p.Start,
                p.End,
                p.OrgId
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return balance ?? throw new KeyNotFoundException("Balance not found");
    }

    public async Task<BalanceDto> CreateBalanceAsync(CreateBalanceRequest request)
    {
        var resp = dbCtx.Balances.Add(new Balance
        {
            OrgId = JwtDto.OrgId,
            Year = request.Year,
            Month = request.Month,
            Charges = request.Charges,
            Payments = request.Payments,
            Fines = request.Fines,
            Start = request.Start,
            End = request.End,
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new BalanceDto(
                resp.Entity.Id,
                resp.Entity.Year,
                resp.Entity.Month,
                resp.Entity.Charges,
                resp.Entity.Payments,
                resp.Entity.Fines,
                resp.Entity.Start,
                resp.Entity.End,
                resp.Entity.OrgId
            )
            : throw new ArgumentException("Failed to create Balance");
    }

    public async Task UpdateBalanceAsync(Guid id, UpdateBalanceRequest request)
    {
        var balance = await dbCtx.Balances
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (balance == null)
            throw new KeyNotFoundException("Balance not found");

        balance.Year = request.Year ?? balance.Year;
        balance.Month = request.Month ?? balance.Month;
        balance.Charges = request.Charges != 0 ? request.Charges : balance.Charges;
        balance.Payments = request.Payments != 0 ? request.Payments : balance.Payments;
        balance.Fines = request.Fines != 0 ? request.Fines : balance.Fines;
        balance.Start = request.Start != 0 ? request.Start : balance.Start;
        balance.End = request.End != 0 ? request.End : balance.End;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteBalanceAsync(Guid id)
    {
        var balance = await dbCtx.Balances
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (balance == null)
            throw new KeyNotFoundException("Balance not found");

        dbCtx.Balances.Remove(balance);
        await dbCtx.SaveChangesAsync();
    }
}
