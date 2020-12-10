using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task<Payment> GetByBankingPaymentIdAsync(string bankingPaymentId);
        Task<List<Payment>> GetPaymentsByBankingPaymentIdAsync(IEnumerable<string> ids);
        Task CreateAsync(Payment payment);
        Task SaveChangesAsync();
    }
}
