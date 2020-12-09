using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Checkout.PaymentGateway.Infrastructure.DbContext
{
    public class PaymentGatewayDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PaymentGatewayDbContext()
            : base()
        {
        }

        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentGatewayDbContext).Assembly);
        }
    }
}
