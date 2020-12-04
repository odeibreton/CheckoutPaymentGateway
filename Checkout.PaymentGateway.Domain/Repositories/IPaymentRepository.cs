using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(PaymentId id);
        Task CreateAsync(Payment payment);
        Task SaveAsync();
    }
}
