using System;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class ApplicationDbContextFactory
    {
        private readonly string _dbName;

        public ApplicationDbContextFactory()
        {
            this._dbName = Guid.NewGuid().ToString();
        }

        public async Task<IApplicationDbContext> Create(Func<IApplicationDbContext, Task> seederFunction = null)
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(this._dbName)
                .Options;
            var context = new TestAppDbContext(options);

            context.Database.EnsureCreated();

            if (seederFunction != null)
            {
                await seederFunction(context);
            }

            return context;
        }
    }

    public class TestAppDbContext : DbContext, IApplicationDbContext
    {
        public TestAppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
