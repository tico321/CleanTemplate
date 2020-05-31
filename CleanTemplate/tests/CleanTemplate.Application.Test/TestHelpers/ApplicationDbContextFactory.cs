using System;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class ApplicationDbContextFactory
    {
        private readonly string _dbName;

        public ApplicationDbContextFactory()
        {
            _dbName = Guid.NewGuid().ToString();
        }

        public async Task<IApplicationDbContext> Create(Func<IApplicationDbContext, Task> seederFunction = null)
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(_dbName)
                .Options;
            var context = new ApplicationDbContext(options, new FakeUserService(), new FixedDateTimeProvider());

            context.Database.EnsureCreated();

            if (seederFunction != null)
            {
                await seederFunction(context);
            }

            return context;
        }
    }
}
