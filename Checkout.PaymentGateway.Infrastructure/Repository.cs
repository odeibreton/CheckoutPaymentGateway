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
        private IDbContextTransaction _transaction;
        private readonly ILogger<Repository> _logger;

        protected PaymentGatewayDbContext DbContext { get; }
        private IDbContextTransaction Transaction
        {
            get => _transaction;
            set
            {
                if (_transaction != null)
                {
                    throw new InvalidOperationException("Cannot create a new transaction while another one is in progress.");
                }

                _transaction = value;
            }
        }

        protected Repository(PaymentGatewayDbContext dbContext, ILogger<Repository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task SaveAsync()
        {
            if (Transaction is null)
            {
                return;
            }

            try
            {
                await Transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await Transaction.RollbackAsync();
                _logger.LogError(e, "Database transaction commit failed.");
                throw;
            }
            finally
            {
                await Transaction.DisposeAsync();
                Transaction = null;
            }
        }

        protected async Task BeginTransactionAsync()
        {
            Transaction = await DbContext.Database.BeginTransactionAsync();
        }
    }
}
