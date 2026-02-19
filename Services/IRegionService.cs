using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IRegionService
{
    Task<LoadResult> GetRegionListAsync(MyDataSourceLoadOptions loadOptions);
    Task<RegionResponse> GetRegionByIdAsync(Guid id);
    Task<RegionResponse> CreateRegionAsync(CreateRegionRequest request);
    Task UpdateRegionAsync(Guid id, UpdateRegionRequest request);
    Task DeleteRegionAsync(Guid id);
}
