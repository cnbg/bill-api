using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IBalanceService
{
    Task<LoadResult> GetBalanceListAsync(MyDataSourceLoadOptions loadOptions);
    Task<BalanceDto> GetBalanceByIdAsync(Guid id);
    Task<BalanceDto> CreateBalanceAsync(CreateBalanceRequest request);
    Task UpdateBalanceAsync(Guid id, UpdateBalanceRequest request);
    Task DeleteBalanceAsync(Guid id);
}
