using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Brands;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Persistence.Configurations;
public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes", "tasksMgmt");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.ChatId)
            .HasColumnName("ChatId")
            .IsRequired();
        
        builder.HasOne<Brand>()
            .WithMany()
            .HasForeignKey(n => n.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(n => n.Status)
            .HasConversion(
                status => status.Code,
                code => NoteStatus.Of(code))
            .HasColumnName("Status")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property<string>(n => n.Content)
            .HasColumnName("Content")
            .HasMaxLength(500);
        
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