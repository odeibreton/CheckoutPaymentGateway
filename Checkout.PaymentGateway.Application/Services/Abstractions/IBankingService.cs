using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Domain;

namespace Checkout.PaymentGateway.Application.Services.Abstractions
{
    public interface IBankingService
    {
        Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation);
    }
}
