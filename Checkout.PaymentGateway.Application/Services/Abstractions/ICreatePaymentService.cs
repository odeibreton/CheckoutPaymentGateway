using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;

namespace Checkout.PaymentGateway.Application.Services.Abstractions
{
    public interface ICreatePaymentService
    {
        Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation);
    }
}