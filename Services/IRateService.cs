using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IRateService
{
    Task<LoadResult> GetRateListAsync(MyDataSourceLoadOptions loadOptions);
    Task<RateDto> GetRateByIdAsync(Guid id);
    Task<RateDto> CreateRateAsync(CreateRateRequest request);
    Task UpdateRateAsync(Guid id, UpdateRateRequest request);
    Task DeleteRateAsync(Guid id);
}
