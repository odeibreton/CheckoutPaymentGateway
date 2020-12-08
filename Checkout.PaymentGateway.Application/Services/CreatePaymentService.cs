using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Commands;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Services
{
    public class CreatePaymentService : ICreatePaymentService
    {
        private readonly IMessageDispatcher _dispatcher;
        private readonly IBankingService _bankingService;
        private readonly ILogger<CreatePaymentService> _logger;

        public CreatePaymentService(IMessageDispatcher dispatcher, IBankingService bankingService, ILogger<CreatePaymentService> logger)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BankingPaymentResult> MakePaymentAsync(PaymentInformation paymentInformation)
        {
            _ = paymentInformation ?? throw new ArgumentNullException(nameof(paymentInformation));

            var result = await SubmitPaymentAsync(paymentInformation);

            var command = new CreatePayment()
            {
                BankingPaymentId = result.Id,
                Successful = result.Successful,
                CardNumber = paymentInformation.CardNumber,
                ExpiryMonth = paymentInformation.ExpiryMonth,
                ExpiryYear = paymentInformation.ExpiryYear,
                CVV = paymentInformation.CVV,
                Amount = paymentInformation.Amount,
                Currency = paymentInformation.Currency
            };

            await _dispatcher.DispatchAsync(command);

            return result;
        }

        private async Task<BankingPaymentResult> SubmitPaymentAsync(PaymentInformation paymentInformation)
        {
            try
            {
                var paymentResult = await _bankingService.MakePaymentAsync(paymentInformation);
                return paymentResult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing payment.");
                throw new BankingException("Error processing payment.", e);
            }
        }
    }
}
