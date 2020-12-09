using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Commands;

namespace Checkout.PaymentGateway.Application.Handlers.CreatePayment
{
    public sealed class EncryptCreatePaymentDecorator : EncryptDecorator<Domain.Commands.CreatePayment>
    {
        public EncryptCreatePaymentDecorator(ICommandHandler<Domain.Commands.CreatePayment> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler, encryptionService)
        {
        }

        protected override Task HandleDecoratorAsync(Domain.Commands.CreatePayment command)
        {
            command.CardNumber = Encrypt(command.CardNumber);
            return Task.CompletedTask;
        }
    }
}
