using Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos").HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.Title).HasColumnName("title").IsRequired().HasMaxLength(200);
        builder
            .Property(t => t.Completed)
            .HasColumnName("completed")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();

        builder
            .HasGeneratedTsVectorColumn(t => t.SearchVector, "english", b => new { b.Title })
            .HasIndex(b => b.SearchVector)
            .HasMethod("GIN");
    }
}
