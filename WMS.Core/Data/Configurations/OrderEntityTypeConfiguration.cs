using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Data.Configurations.Shared;
using WMS.Models.Entities;

namespace WMS.Core.Data.Configurations;

public class OrderEntityTypeConfiguration: BaseEntityTypeConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);

        builder.HasMany(order => order.Items)
            .WithOne(orderItem => orderItem.Order)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}