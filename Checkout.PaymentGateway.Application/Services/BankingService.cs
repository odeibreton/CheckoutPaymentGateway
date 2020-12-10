using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.Application.Services
{
    public class BankingService : IBankingService, IDisposable
    {
        private BankingServiceOptions Options { get; }
        private HttpClient HttpClient { get; }

        public BankingService(IOptions<BankingServiceOptions> options, HttpClient httpClient)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation)
        {
            var result = await HttpClient.PostAsJsonAsync(Options.PaymentEndpoint, paymentInformation);

            result.EnsureSuccessStatusCode();

            return await result.Content.ReadFromJsonAsync<BankingPaymentResult>();
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }

    public class BankingServiceOptions
    {
        public string BaseAddress { get; set; }
        public string PaymentEndpoint { get; set; }
    }
}
