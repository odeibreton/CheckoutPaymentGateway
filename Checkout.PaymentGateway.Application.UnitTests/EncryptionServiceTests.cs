using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class EncryptionServiceTests : IDisposable
    {
        #region Creation
        private RSA RSAService { get; } = RSA.Create();
        private IEncryptionService Service { get; }

        public EncryptionServiceTests()
        {
            Service = new EncryptionService(RSAService);
        }
        #endregion

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EncryptionService(null));
        }

        [Theory]
        [InlineData("TEST DATA")]
        [InlineData("4485388069721426")]
        [InlineData("")]
        public void ShouldEncryptAndDecrypt(string data)
        {
            var pu = Convert.ToBase64String(RSAService.ExportRSAPublicKey());
            var pr = Convert.ToBase64String(RSAService.ExportRSAPrivateKey());

            var result = Service.Decrypt(Service.Encrypt(data));
            Assert.Equal(data, result);
        }

        public void Dispose()
        {
            Service.Dispose();
        }
    }
}
