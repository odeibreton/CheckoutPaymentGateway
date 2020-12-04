using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain;
using Checkout.PaymentGateway.Domain.Repositories;
using Checkout.PaymentGateway.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentGatewayDbContext _dbContext;
        private readonly ILogger<PaymentRepository> _logger;
        private IDbContextTransaction _transaction;

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

        public PaymentRepository(PaymentGatewayDbContext dbContext, ILogger<PaymentRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Payment> GetByIdAsync(PaymentId id)
        {
            return _dbContext.Payments.FindAsync(id).AsTask();
        }

        public async Task CreateAsync(Payment payment)
        {
            await BeginTransactionAsync();
            await _dbContext.Payments.AddAsync(payment);
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

        private async Task BeginTransactionAsync()
        {
            Transaction = await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
