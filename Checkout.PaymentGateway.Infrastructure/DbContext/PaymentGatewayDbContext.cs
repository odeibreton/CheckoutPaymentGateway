using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.DbContext
{
    public class PaymentGatewayDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PaymentGatewayDbContext()
            : base()
        {
        }

        public PaymentGatewayDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
