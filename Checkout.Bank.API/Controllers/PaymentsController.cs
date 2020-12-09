using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Bank.API.Configuration;
using Checkout.Bank.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Checkout.Bank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private PaymentOptions Options { get; }

        public PaymentsController(IOptions<PaymentOptions> options)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResult>> PostPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(Options.PaymentDelay);

            if (Options.InvalidCards.Contains(request.CardNumber))
            {
                return new PaymentResult()
                {
                    Id = NewId(),
                    Successful = false,
                    Error = "Invalid card."
                };
            }

            return new PaymentResult()
            {
                Id = NewId(),
                Successful = true
            };
        }

        private static string NewId() => Guid.NewGuid().ToString();
    }
}
