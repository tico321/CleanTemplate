using CleanTemplate.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Infrastructure.Persistence.Configurations
{
  public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
  {
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
      builder.Property(i => i.Description).HasMaxLength(maxLength: 50);
    }
  }
}