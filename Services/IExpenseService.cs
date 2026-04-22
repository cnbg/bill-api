using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IExpenseService
{
    Task<LoadResult> GetExpenseListAsync(MyDataSourceLoadOptions loadOptions);
    Task<ExpenseDto> GetExpenseByIdAsync(Guid id);
    Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request);
    Task UpdateExpenseAsync(Guid id, UpdateExpenseRequest request);
    Task DeleteExpenseAsync(Guid id);
}
