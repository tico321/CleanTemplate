using System;
using System.Linq;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.Todos.Model;
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

            _sut.TodoLists.Add(new TodoList("userId", "list", 1).SequenceAddTodo("item1"));

            _sut.SaveChangesAsync().Wait();
        }

        public void Dispose()
        {
            _sut?.Dispose();
        }

        private readonly string _userId;
        private readonly ApplicationDbContext _sut;
        private readonly DateTime _dateTime = new DateTime(3001, 1, 1);

        [Fact]
        public async Task SaveChangesAsync_GivenExistingTodoItem_ShouldSetLastModifiedProperties()
        {
            var list = _sut.TodoLists.First();
            list.Description = "new description";
            var item = list.Todos.First();
            item.Description = "new description";
            // Assert DbContext created the list correctly
            Assert.NotNull(list.CreatedBy);
            Assert.Equal(_dateTime, list.Created);
            Assert.NotNull(item.CreatedBy);
            Assert.Equal(_dateTime, item.Created);

            await _sut.SaveChangesAsync();

            Assert.NotNull(list.LastModified);
            Assert.Equal(_dateTime, list.LastModified);
            Assert.Equal(_userId, list.LastModifiedBy);
            Assert.Equal(_dateTime, item.LastModified);
            Assert.Equal(_userId, item.LastModifiedBy);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewTodoItem_ShouldSetCreatedProperties()
        {
            var item = new TodoItem("other task", 2);

            await _sut.TodoItems.AddAsync(item);

            await _sut.SaveChangesAsync();

            Assert.Equal(_dateTime, item.Created);
            Assert.Equal(_userId, item.CreatedBy);
            Assert.Null(item.LastModified);
            Assert.Null(item.LastModifiedBy);

            // If we don't make changes auditable entities should not be updated
            item = _sut.TodoItems.First(i => i.Description == item.Description);

            await _sut.SaveChangesAsync();

            Assert.Null(item.LastModified);
            Assert.Null(item.LastModifiedBy);
        }
    }
}
