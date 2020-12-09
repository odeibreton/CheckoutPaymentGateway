using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure
{
    public abstract class Repository
    {
        private readonly ILogger<Repository> _logger;

        protected PaymentGatewayDbContext DbContext { get; }

        protected Repository(PaymentGatewayDbContext dbContext, ILogger<Repository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Changes could not be saved in the database.");
                throw;
            }
        }
    }
}
