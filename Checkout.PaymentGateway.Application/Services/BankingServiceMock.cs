using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.Application.Services
{
    public sealed class BankingServiceMock : IBankingService
    {
        private BankingServiceMockOptions Options { get; }

        public BankingServiceMock(IOptions<BankingServiceMockOptions> options)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<MakeBankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation)
        {
            _ = paymentInformation ?? throw new ArgumentNullException(nameof(paymentInformation));

            if (Options.InvalidCards.Contains(paymentInformation.CardNumber))
            {
                return Task.FromResult(new MakeBankingPaymentResult()
                {
                    Id = GenerateId(),
                    Successful = false,
                    Error = "Invalid card."
                });
            }

            return Task.FromResult(new MakeBankingPaymentResult()
            {
                Id = GenerateId(),
                Successful = true
            });
        }

        private static string GenerateId() => Guid.NewGuid().ToString();
    }

    public sealed class BankingServiceMockOptions
    {
        public List<string> InvalidCards { get; set; } = new List<string>();
    }
}
