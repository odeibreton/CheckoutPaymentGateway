using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class Payment : AggregateRoot<int>
    {
        private Payment(int id) : base(id)
        {
        }

        public Payment(int id,
                       string bankingPaymentId,
                       bool successfulPayment,
                       CardNumber cardNumber,
                       int expiryMonth,
                       int expiryYear,
                       CVV cvv,
                       decimal amount,
                       Currency currency)
            : this(id)
        {
            SuccessfulPayment = successfulPayment;
            BankingPaymentId = bankingPaymentId ?? throw new ArgumentNullException(nameof(bankingPaymentId));
            CardNumber = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CVV = cvv ?? throw new ArgumentNullException(nameof(cvv));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public string BankingPaymentId { get; private set; }
        public bool SuccessfulPayment { get; private set; }
        public CardNumber CardNumber { get; private set; }
        public int ExpiryMonth { get; private set; }
        public int ExpiryYear { get; private set; }
        public CVV CVV { get; private set; }
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
    }
}
