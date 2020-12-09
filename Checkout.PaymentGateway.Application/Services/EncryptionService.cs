using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Checkout.PaymentGateway.Application.Services.Abstractions;

namespace Checkout.PaymentGateway.Application.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly RSA _rsa;

        public EncryptionService(RSA rsa)
        {
            _rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
        }

        public string Encrypt(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var encrypted = _rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string value)
        {
            var data = Convert.FromBase64String(value);
            var encrypted = _rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(encrypted);
        }

        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
