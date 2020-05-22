using System.Reflection;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Will register all the configurations that are defined in Persistence/Configurations
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
