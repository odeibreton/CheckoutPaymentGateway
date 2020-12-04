using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.PaymentGateway.Infrastructure.DbContext
{
    internal sealed class PaymentsConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasConversion(id => id.Id, id => new PaymentId(id));

            builder.OwnsOne(p => p.CardNumber)
                .Property(c => c.Value)
                .HasColumnName("CardNumber")
                .IsRequired();

            builder.OwnsOne(p => p.CVV)
                .Property(cvv => cvv.Value)
                .HasColumnName("CVV")
                .IsRequired();

            builder.OwnsOne(p => p.Currency)
                .Property(c => c.Value)
                .HasColumnName("Currency")
                .IsRequired();
        }
    }
}
