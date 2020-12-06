using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain;
using Checkout.PaymentGateway.Domain.Commands;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public sealed class CreatePaymentHandler : ICommandHandler<CreatePayment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankingService _bankingService;
        private readonly ILogger<CreatePaymentHandler> _logger;

        public CreatePaymentHandler(IPaymentRepository paymentRepository, IBankingService bankingService, ILogger<CreatePaymentHandler> logger)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(CreatePayment command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            var paymentInformation = new PaymentInformation()
            {
                CardNumber = command.CardNumber,
                ExpityMonth = command.ExpityMonth,
                ExpiryYear = command.ExpiryYear,
                CVV = command.CVV,
                Amount = command.Amount,
                Currency = command.Currency
            };

            var paymentResult = await SubmitPaymentAsync(paymentInformation);
            await ProcessPaymentAsync(command, paymentResult);

            if (!paymentResult.Successful)
            {
                throw new BankingException(paymentResult.Error);
            }
        }

        private async Task<MakeBankingPaymentResult> SubmitPaymentAsync(PaymentInformation paymentInformation)
        {
            try
            {
                var paymentId = await _bankingService.MakePayment(paymentInformation);
                return paymentId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing payment.");
                throw new BankingException("Error processing payment.", e);
            }
        }

        private async Task ProcessPaymentAsync(CreatePayment command, MakeBankingPaymentResult paymentResult)
        {
            try
            {
                var payment = new Payment(null,
                                          paymentResult.Id,
                                          paymentResult.Successful,
                                          new CardNumber(command.CardNumber),
                                          command.ExpityMonth,
                                          command.ExpiryYear,
                                          new CVV(command.CVV),
                                          command.Amount,
                                          new Currency(command.Currency)
                );

                await _paymentRepository.CreateAsync(payment);
                await _paymentRepository.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Payment could not be saved.", paymentResult);
                throw e;
            }
        }
    }
}
