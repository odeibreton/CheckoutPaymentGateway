using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.Application.Services
{
    public class BankingService : IBankingService, IDisposable
    {
        private BankingServiceOptions Options { get; }
        private HttpClient HttpClient { get; }
        private ILogger<BankingService> Logger { get; }

        public BankingService(IOptions<BankingServiceOptions> options, HttpClient httpClient, ILogger<BankingService> logger)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation)
        {
            Logger.LogInformation("Triggering payment request to bank.");
            var result = await HttpClient.PostAsJsonAsync(Options.PaymentEndpoint, paymentInformation);

            try
            {
                result.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Bank returned error on payment request.");
                throw e;
            }

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
