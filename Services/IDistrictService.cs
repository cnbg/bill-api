using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IDistrictService
{
    Task<LoadResult> GetDistrictListAsync(MyDataSourceLoadOptions loadOptions);
    Task<DistrictResponse> GetDistrictByIdAsync(Guid id);
    Task<DistrictResponse> CreateDistrictAsync(CreateDistrictRequest request);
    Task UpdateDistrictAsync(Guid id, UpdateDistrictRequest request);
    Task DeleteDistrictAsync(Guid id);
}
