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
        private readonly ILogger<CreatePaymentHandler> _logger;

        public CreatePaymentHandler(IPaymentRepository paymentRepository, ILogger<CreatePaymentHandler> logger)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(CreatePayment command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            try
            {
                var payment = new Payment(new PaymentId(0),
                                          command.BankingPaymentId,
                                          command.Successful,
                                          new CardNumber(command.CardNumber),
                                          command.ExpiryMonth,
                                          command.ExpiryYear,
                                          new CVV(command.CVV),
                                          command.Amount,
                                          new Currency(command.Currency)
                );

                await _paymentRepository.CreateAsync(payment);
                await _paymentRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Payment could not be saved.", command.BankingPaymentId);
                throw e;
            }
        }
    }
}
