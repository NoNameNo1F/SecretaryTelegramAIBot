using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Brands;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Persistence.Configurations;

public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", "tasksMgmt");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(b => b.NormalizedName)
            .HasColumnName("NormalizedName")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property<DateTime>(n => n.CreatedAt)
            .HasColumnName("CreatedAt");
        
        builder.Property<DateTime?>(n => n.UpdatedAt)
            .HasColumnName("UpdatedAt");
        
        builder.Property<DateTime?>(n => n.DeletedAt)
            .HasColumnName("DeletedAt");
        
        builder.Property<bool>(n => n.IsDeleted)
            .HasColumnName("IsDeleted");
    }
}