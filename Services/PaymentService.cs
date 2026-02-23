using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class PaymentService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : IPaymentService
{
    private readonly JwtClaim _jwtDto = (ctxAccessor.HttpContext ?? throw new InvalidOperationException()).GetJwtClaims();

    public async Task<LoadResult> GetPaymentListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Payments
            .Include(p => p.Org)
            .Where(p => p.OrgId == _jwtDto.OrgId)
            // .Select(p => new PaymentDto(
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

    public async Task<PaymentDto> GetPaymentByIdAsync(Guid id)
    {
        var payment = await dbCtx.Payments
            .Include(p => p.Org)
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .Select(p => new PaymentDto(
                p.Id,
                p.Account,
                p.Amount,
                p.Source,
                p.Status,
                p.Year,
                p.Month,
                p.Note,
                p.OrgId
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return payment ?? throw new KeyNotFoundException("Payment not found");
    }

    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentRequest request)
    {
        var resp = dbCtx.Payments.Add(new Payment
        {
            OrgId = _jwtDto.OrgId,
            Account = request.Account,
            Amount = request.Amount,
            Source = request.Source,
            Status = request.Status,
            Year = request.Year,
            Month = request.Month,
            Note = request.Note ?? string.Empty
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new PaymentDto(
                resp.Entity.Id,
                resp.Entity.Account,
                resp.Entity.Amount,
                resp.Entity.Source,
                resp.Entity.Status,
                resp.Entity.Year,
                resp.Entity.Month,
                resp.Entity.Note,
                resp.Entity.OrgId
            )
            : throw new ArgumentException("Failed to create payment");
    }

    public async Task UpdatePaymentAsync(Guid id, UpdatePaymentRequest request)
    {
        var payment = await dbCtx.Payments
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        payment.Account = request.Account ?? payment.Account;
        payment.Amount = request.Amount ?? payment.Amount;
        payment.Source = request.Source ?? payment.Source;
        payment.Status = request.Status ?? payment.Status;
        payment.Year = request.Year ?? payment.Year;
        payment.Month = request.Month ?? payment.Month;
        payment.Note = request.Note ?? payment.Note;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(Guid id)
    {
        var payment = await dbCtx.Payments
            .Where(p => p.Id == id && p.OrgId == _jwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        dbCtx.Payments.Remove(payment);
        await dbCtx.SaveChangesAsync();
    }
}
