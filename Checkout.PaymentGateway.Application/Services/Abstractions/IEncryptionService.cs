using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application.Services.Abstractions
{
    public interface IEncryptionService : IDisposable
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
