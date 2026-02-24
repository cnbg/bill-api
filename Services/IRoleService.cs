using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IRoleService
{
    Task<LoadResult> GetRoleListAsync(MyDataSourceLoadOptions loadOptions);
    Task<RoleDto> GetRoleByIdAsync(Guid id);
    Task<RoleDto> CreateRoleAsync(CreateRoleRequest request);
    Task UpdateRoleAsync(Guid id, UpdateRoleRequest request);
    Task UpdateRolePermAsync(Guid id, UpdateRolePermRequest request);
    Task DeleteRoleAsync(Guid id);
}
