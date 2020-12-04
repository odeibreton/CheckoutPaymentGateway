using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class Payment : AggregateRoot<PaymentId>
    {
        public Payment(PaymentId id) : base(id)
        {
        }

        public Payment(PaymentId id,
                       CardNumber cardNumber,
                       int expityMonth,
                       int expiryYear,
                       CVV cvv,
                       decimal amount,
                       Currency currency)
            : this(id)
        {
            CardNumber = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
            ExpiryMonth = expityMonth;
            ExpiryYear = expiryYear;
            CVV = cvv ?? throw new ArgumentNullException(nameof(cvv));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public CardNumber CardNumber { get; private set; }
        public int ExpiryMonth { get; private set; }
        public int ExpiryYear { get; private set; }
        public CVV CVV { get; private set; }
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
    }
}
