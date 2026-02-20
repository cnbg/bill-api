using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IOrgService
{
    Task<LoadResult> GetOrgListAsync(MyDataSourceLoadOptions loadOptions);
    Task<OrgDto> GetOrgByIdAsync(Guid id);
    Task<OrgDto> CreateOrgAsync(CreateOrgRequest request);
    Task UpdateOrgAsync(Guid id, UpdateOrgRequest request);
    Task DeleteOrgAsync(IEnumerable<Guid> ids);
}
