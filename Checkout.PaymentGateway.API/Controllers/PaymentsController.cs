using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentGateway.API.Model;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ICreatePaymentService _createPaymentService;
        private readonly IMessageDispatcher _dispatcher;

        public PaymentsController(ICreatePaymentService createPaymentService, IMessageDispatcher dispatcher)
        {
            _createPaymentService = createPaymentService ?? throw new ArgumentNullException(nameof(createPaymentService));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        [HttpGet("{bankingPaymentId}")]
        public async Task<ActionResult<GetPaymentByBankingPaymentIdResult>> GetPayment(string bankingPaymentId)
        {
            var query = new GetPaymentByBankingPaymentId()
            {
                BankingPaymentId = bankingPaymentId
            };

            var result = await _dispatcher.DispatchAsync<GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult>(query);

            if (result is null)
                return NotFound();

            return result;
        }

        [HttpPost("get")]
        public async Task<ActionResult<GetPaymentsByBankingPaymentIdResult>> GetPayment(GetPaymentsRequest request)
        {
            var query = new GetPaymentsByBankingPaymentId()
            {
                Ids = request.Ids
            };

            var result = await _dispatcher.DispatchAsync<GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult>(query);

            return result;
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
