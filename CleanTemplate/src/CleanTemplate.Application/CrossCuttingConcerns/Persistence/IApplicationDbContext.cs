using System;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Model;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.CrossCuttingConcerns.Persistence
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<TodoList> TodoLists { get; set; } // we only expose AggregateRoots
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
