﻿using Shopway.Domain.Enums;
using Shopway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Converters.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Shopway.Domain.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableName.Payment, SchemaName.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<PaymentIdConverter, PaymentIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(p => p.OrderHeaderId)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(p => p.Status)
            .HasConversion<PaymentStatusConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<PaymentStatus>()))
            .IsRequired(true);

        builder.ConfigureAuditableEntity();
    }
}