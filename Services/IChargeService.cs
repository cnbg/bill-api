using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IChargeService
{
    Task<LoadResult> GetChargeListAsync(MyDataSourceLoadOptions loadOptions);
    Task<ChargeDto> GetChargeByIdAsync(Guid id);
    Task<ChargeDto> CreateChargeAsync(CreateChargeRequest request);
    Task UpdateChargeAsync(Guid id, UpdateChargeRequest request);
    Task DeleteChargeAsync(Guid id);
}
