﻿using System;
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

namespace Checkout.PaymentGateway.Application.Handlers.CreatePayment
{
    [Encrypt(typeof(Domain.Commands.CreatePayment))]
    public sealed class Handler : ICommandHandler<Domain.Commands.CreatePayment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<Handler> _logger;

        public Handler(IPaymentRepository paymentRepository, ILogger<Handler> logger)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(Domain.Commands.CreatePayment command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            try
            {
                var payment = new Payment(0,
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
