using System;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Domain.Todos;
using CleanTemplate.Infrastructure.Persistence;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CleanTemplate.Infrastructure.IntegrationTest.Persistence
{
    public class ApplicationDbContextTests : IDisposable
    {
        public ApplicationDbContextTests()
        {
            var dateTimeProvider = A.Fake<IDateTime>();
            A.CallTo(() => dateTimeProvider.Now).Returns(_dateTime);

            _userId = "00000000-0000-0000-0000-000000000000";
            var currentUserServiceProvider = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserServiceProvider.UserId).Returns(_userId);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _sut = new ApplicationDbContext(options, currentUserServiceProvider, dateTimeProvider);

            _sut.TodoItems.Add(new TodoItem {Id = ItemId, Description = "Do this thing."});

            _sut.SaveChanges();
        }

        public void Dispose()
        {
            _sut?.Dispose();
        }

        private readonly string _userId;
        private readonly ApplicationDbContext _sut;
        private readonly long ItemId = 1;
        private readonly DateTime _dateTime = new DateTime(year: 3001, month: 1, day: 1);

        [Fact]
        public async Task SaveChangesAsync_GivenExistingTodoItem_ShouldSetLastModifiedProperties()
        {
            var item = await _sut.TodoItems.FindAsync(ItemId);
            item.Description = "new description";

            await _sut.SaveChangesAsync();

            Assert.NotNull(item.LastModified);
            Assert.Equal(_dateTime, item.LastModified);
            Assert.Equal(_userId, item.LastModifiedBy);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewTodoItem_ShouldSetCreatedProperties()
        {
            var item = new TodoItem {Id = 2, Description = "This thing is done."};

            _sut.TodoItems.Add(item);

            await _sut.SaveChangesAsync();

            Assert.Equal(_dateTime, item.Created);
            Assert.Equal(_userId, item.CreatedBy);
        }

        [Fact]
        public async Task SaveChangesAsync_WithNoChanges_ShouldNotUpdateAuditableProperties()
        {
            var item = await _sut.TodoItems.FindAsync(ItemId);

            await _sut.SaveChangesAsync();

            Assert.Null(item.LastModified);
            Assert.Null(item.LastModifiedBy);
        }
    }
}
