using CleanTemplate.Application.Infrastructure;
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
	}
}
