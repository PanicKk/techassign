using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Data.Configurations.Shared;
using WMS.Models.Entities;

namespace WMS.Core.Data.Configurations;

public class WebhookEntityTypeConfiguration : BaseEntityTypeConfiguration<Webhook>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Webhook> builder)
    {
        builder.HasKey(webhook => webhook.Id);
    }
}