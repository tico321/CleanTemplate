using CleanTemplate.Application.Todos.Model;
using CleanTemplate.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Infrastructure.Persistence.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Description).HasMaxLength(200);
            builder.Property(i => i.State)
                .HasColumnType("varchar(32)")
                .HasConversion(
                    s => s.Name,
                    name => Enumeration.FromDisplayName<TodoItemState>(name));
        }
    }
}
