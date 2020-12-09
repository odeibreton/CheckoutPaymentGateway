using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
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

        public async Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation)
        {
            _ = paymentInformation ?? throw new ArgumentNullException(nameof(paymentInformation));

            await Task.Delay(Options.PaymentDelay);

            if (Options.InvalidCards.Contains(paymentInformation.CardNumber))
            {
                return new BankingPaymentResult()
                {
                    Id = GenerateId(),
                    Successful = false,
                    Error = "Invalid card."
                };
            }

            return new BankingPaymentResult()
            {
                Id = GenerateId(),
                Successful = true
            };
        }

        private static string GenerateId() => Guid.NewGuid().ToString();
    }

    public sealed class BankingServiceMockOptions
    {
        public List<string> InvalidCards { get; set; } = new List<string>();
        public int PaymentDelay { get; set; }
    }
}
