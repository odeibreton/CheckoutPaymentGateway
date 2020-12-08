using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentGateway.API.Model;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ICreatePaymentService _createPaymentService;

        public PaymentsController(ICreatePaymentService createPaymentService)
        {
            _createPaymentService = createPaymentService ?? throw new ArgumentNullException(nameof(createPaymentService));
        }

        [HttpPost]
        public async Task<ActionResult<BankingPaymentResult>> MakePayment(MakePaymentRequest payment)
        {
            var paymentInformation = new PaymentInformation()
            {
                CardNumber = payment.CardNumber,
                ExpiryMonth = payment.ExpiryMonth.Value,
                ExpiryYear = payment.ExpiryYear.Value,
                CVV = payment.CVV,
                Amount = payment.Amount.Value,
                Currency = payment.Currency
            };

            return await _createPaymentService.MakePaymentAsync(paymentInformation);
        }
    }
}
