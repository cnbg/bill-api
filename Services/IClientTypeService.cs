using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IClientTypeService
{
    Task<LoadResult> GetClientTypeListAsync(MyDataSourceLoadOptions loadOptions);
    Task<ClientTypeDto> GetClientTypeByIdAsync(Guid id);
    Task<ClientTypeDto> CreateClientTypeAsync(CreateClientTypeRequest request);
    Task UpdateClientTypeAsync(Guid id, UpdateClientTypeRequest request);
    Task DeleteClientTypeAsync(Guid id);
}
