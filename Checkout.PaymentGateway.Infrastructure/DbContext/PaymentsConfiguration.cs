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
                .ValueGeneratedOnAdd();

            builder.Property(p => p.BankingPaymentId)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(p => p.CardNumber)
                .HasConversion(v => v.Value, v => new CardNumber(v))
                .HasColumnName("CardNumber")
                .HasMaxLength(19)
                .IsRequired();

            builder.Property(p => p.CVV)
                .HasConversion(v => v.Value, v => new CVV(v))
                .HasColumnName("CVV")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(p => p.Currency)
                .HasConversion(v => v.Value, v => new Currency(v))
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnType("DECIMAL(6,3)");
        }
    }
}
