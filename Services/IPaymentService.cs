using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IPaymentService
{
    Task<LoadResult> GetPaymentListAsync(MyDataSourceLoadOptions loadOptions);
    Task<PaymentDto> GetPaymentByIdAsync(Guid id);
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentRequest request);
    Task UpdatePaymentAsync(Guid id, UpdatePaymentRequest request);
    Task DeletePaymentAsync(IEnumerable<Guid> ids);
}
