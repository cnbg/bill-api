using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class ChargeService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : IChargeService
{
    private readonly JwtClaim _jwtDto = (ctxAccessor.HttpContext ?? throw new InvalidOperationException()).GetJwtClaims();

    public async Task<LoadResult> GetChargeListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Charges
            .Include(p => p.Org)
            .Where(p => p.OrgId == _jwtDto.OrgId)
            // .Select(p => new ChargeDto(
            //     p.Id,
            //     p.Account,
            //     p.Amount,
            //     p.Source,
            //     p.Status,
            //     p.Year,
            //     p.Month,
            //     p.Note,
            //     p.OrgId
            // ))
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<ChargeDto> GetChargeByIdAsync(Guid id)
    {
        var charge = await dbCtx.Charges
            .Include(p => p.Org)
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .Select(p => new ChargeDto(
                p.Id,
                p.Account,
                p.Amount,
                p.Status,
                p.Year,
                p.Month,
                p.Note,
                p.OrgId
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return charge ?? throw new KeyNotFoundException("Charge not found");
    }

    public async Task<ChargeDto> CreateChargeAsync(CreateChargeRequest request)
    {
        var resp = dbCtx.Charges.Add(new Charge
        {
            OrgId = _jwtDto.OrgId,
            Account = request.Account,
            Amount = request.Amount,
            Status = request.Status,
            Year = request.Year,
            Month = request.Month,
            Note = request.Note ?? string.Empty
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new ChargeDto(
                resp.Entity.Id,
                resp.Entity.Account,
                resp.Entity.Amount,
                resp.Entity.Status,
                resp.Entity.Year,
                resp.Entity.Month,
                resp.Entity.Note,
                resp.Entity.OrgId
            )
            : throw new ArgumentException("Failed to create charge");
    }

    public async Task UpdateChargeAsync(Guid id, UpdateChargeRequest request)
    {
        var charge = await dbCtx.Charges
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (charge == null)
            throw new KeyNotFoundException("Charge not found");

        charge.Account = request.Account ?? charge.Account;
        charge.Amount = request.Amount ?? charge.Amount;
        charge.Status = request.Status ?? charge.Status;
        charge.Year = request.Year ?? charge.Year;
        charge.Month = request.Month ?? charge.Month;
        charge.Note = request.Note ?? charge.Note;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteChargeAsync(Guid id)
    {
        var charge = await dbCtx.Charges
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (charge == null)
            throw new KeyNotFoundException("Charge not found");

        dbCtx.Charges.Remove(charge);
        await dbCtx.SaveChangesAsync();
    }
}
