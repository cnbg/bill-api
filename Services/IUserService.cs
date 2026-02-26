using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IUserService
{
    Task<LoadResult> GetUserListAsync(MyDataSourceLoadOptions loadOptions);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task UpdateUserAsync(Guid id, UpdateUserRequest request);
    Task DeleteUserAsync(Guid id);
}
