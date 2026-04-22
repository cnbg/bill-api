using billing.DTOs;
using billing.Entities;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class ExpenseService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : BaseAuthService(ctxAccessor), IExpenseService
{
    public async Task<LoadResult> GetExpenseListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Expenses
            .Include(p => p.Org)
            .Where(p => p.OrgId == JwtDto.OrgId)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<ExpenseDto> GetExpenseByIdAsync(Guid id)
    {
        var expense = await dbCtx.Expenses
            .Include(p => p.Org)
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .Select(p => new ExpenseDto(
                p.Id,
                p.Amount,
                p.Status,
                p.Year,
                p.Month,
                p.Note,
                p.OrgId
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return expense ?? throw new KeyNotFoundException("Expense not found");
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var resp = dbCtx.Expenses.Add(new Expense
        {
            OrgId = JwtDto.OrgId,
            Amount = request.Amount,
            Status = request.Status,
            Year = request.Year,
            Month = request.Month,
            Note = request.Note ?? string.Empty
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new ExpenseDto(
                resp.Entity.Id,
                resp.Entity.Amount,
                resp.Entity.Status,
                resp.Entity.Year,
                resp.Entity.Month,
                resp.Entity.Note,
                resp.Entity.OrgId
            )
            : throw new ArgumentException("Failed to create Expense");
    }

    public async Task UpdateExpenseAsync(Guid id, UpdateExpenseRequest request)
    {
        var expense = await dbCtx.Expenses
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (expense == null)
            throw new KeyNotFoundException("Expense not found");

        expense.Amount = request.Amount ?? expense.Amount;
        expense.Status = request.Status ?? expense.Status;
        expense.Year = request.Year ?? expense.Year;
        expense.Month = request.Month ?? expense.Month;
        expense.Note = request.Note ?? expense.Note;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteExpenseAsync(Guid id)
    {
        var expense = await dbCtx.Expenses
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (expense == null)
            throw new KeyNotFoundException("Expense not found");

        dbCtx.Expenses.Remove(expense);
        await dbCtx.SaveChangesAsync();
    }
}
