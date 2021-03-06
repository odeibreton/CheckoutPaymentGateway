﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain;
using Checkout.PaymentGateway.Domain.Repositories;
using Checkout.PaymentGateway.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure
{
    public class PaymentRepository : Repository, IPaymentRepository
    {
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(PaymentGatewayDbContext dbContext, ILogger<PaymentRepository> logger)
            : base(dbContext, logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Payment> GetByIdAsync(int id)
        {
             return DbContext.Payments.FindAsync(id).AsTask();
        }

        public Task<Payment> GetByBankingPaymentIdAsync(string bankingPaymentId)
        {
            return DbContext.Payments.SingleOrDefaultAsync(p => p.BankingPaymentId == bankingPaymentId);
        }

        public Task<List<Payment>> GetPaymentsByBankingPaymentIdAsync(IEnumerable<string> ids)
        {
            return DbContext.Payments.Where(p => ids.Contains(p.BankingPaymentId)).ToListAsync();
        }

        public Task CreateAsync(Payment payment)
        {
            return DbContext.Payments.AddAsync(payment).AsTask();
        }
    }
}
