using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Data.Configurations.Shared;
using WMS.Models.Entities;

namespace WMS.Core.Data.Configurations;

public class OrderItemEntityTypeConfiguration: BaseEntityTypeConfiguration<OrderItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(orderItem => orderItem.Id);

        builder.HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.Items)
            .OnDelete(DeleteBehavior.Cascade);
    }
}