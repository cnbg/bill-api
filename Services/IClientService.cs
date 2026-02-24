using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IClientService
{
    Task<LoadResult> GetClientListAsync(MyDataSourceLoadOptions loadOptions);
    Task<ClientResponse> GetClientByIdAsync(Guid id);
    Task<ClientResponse> CreateClientAsync(CreateClientRequest request);
    Task UpdateClientAsync(Guid id, UpdateClientRequest request);
    Task DeleteClientAsync(Guid id);
}
