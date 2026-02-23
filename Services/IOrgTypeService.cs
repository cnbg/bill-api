using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IOrgTypeService
{
    Task<LoadResult> GetOrgTypeListAsync(MyDataSourceLoadOptions loadOptions);
    Task<OrgTypeDto> GetOrgTypeByIdAsync(Guid id);
    Task<OrgTypeDto> CreateOrgTypeAsync(CreateOrgTypeRequest request);
    Task UpdateOrgTypeAsync(Guid id, UpdateOrgTypeRequest request);
    Task DeleteOrgTypeAsync(Guid id);
}
