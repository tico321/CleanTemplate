using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.CrossCuttingConcerns
{
	public interface IApplicationDbContext
	{
		DbSet<TodoItem> TodoItems { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
