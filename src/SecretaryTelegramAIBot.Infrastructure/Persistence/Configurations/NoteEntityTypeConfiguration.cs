using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretaryTelegramAIBot.Domain.Entities;

namespace SecretaryTelegramAIBot.Infrastructure.Persistence.Configurations;

public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes", "telegram");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Id)
            .HasConversion(
                id => id.Value,
                value => new NoteId(value))
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property<long>(n => n.ChatId)
            .HasColumnName("ChatId")
            .IsRequired();
        
        builder.Property<string>(n => n.Content)
            .HasColumnName("Content")
            .HasMaxLength(255);
        
        builder.Property<string>(n => n.Brand)
            .HasColumnName("Brand")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property<DateTime>(n => n.CreatedAt)
            .HasColumnName("CreatedAt");
        
        builder.Property<DateTime?>(n => n.UpdatedAt)
            .HasColumnName("UpdatedAt");
    }
}