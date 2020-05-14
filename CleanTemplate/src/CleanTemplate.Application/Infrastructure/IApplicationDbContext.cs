using CleanTemplate.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Application.Infrastructure
{
	public interface IApplicationDbContext
	{
		DbSet<TodoItem> TodoItems { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
