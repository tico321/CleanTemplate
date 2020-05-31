using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.SearchTodoLists;
using FluentValidation.TestHelper;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class SearchTodoListsQueryTest : IClassFixture<RequestTestFixture>
    {
        public SearchTodoListsQueryTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task Search()
        {
            var todos = Enumerable.Range(start: 1, count: 15)
                .Select(i => new TodoList("userId", $"todo {i}", i))
                .ToList();
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var query = new SearchTodoListsQuery {Page = 1, PageSize = 5};
            var handler = new SearchTodoListsQuery.Handler(_fixture.Context, _fixture.Mapper);

            var actual = await handler.Handle(query, CancellationToken.None);

            // first call
            Assert.Equal(expected: 15, actual.TotalItems);
            Assert.Equal(expected: 1, actual.CurrentPage);
            Assert.Equal(expected: 3, actual.EndPage);
            Assert.Equal(expected: 1, actual.StartPage);
            Assert.Equal(expected: 3, actual.TotalPages);
            var data = actual.Data.ToList();
            Assert.Equal(expected: 5, data.Count);
            Assert.Contains(data, d => d.DisplayOrder == 1);
            Assert.Contains(data, d => d.DisplayOrder == 2);
            Assert.Contains(data, d => d.DisplayOrder == 3);
            Assert.Contains(data, d => d.DisplayOrder == 4);
            Assert.Contains(data, d => d.DisplayOrder == 5);

            query.Page = 2;
            actual = await handler.Handle(query, CancellationToken.None);

            // second call
            Assert.Equal(expected: 15, actual.TotalItems);
            Assert.Equal(expected: 2, actual.CurrentPage);
            Assert.Equal(expected: 3, actual.EndPage);
            Assert.Equal(expected: 1, actual.StartPage);
            Assert.Equal(expected: 3, actual.TotalPages);
            Assert.Equal(expected: 5, actual.Data.Count());
            data = actual.Data.ToList();
            Assert.Equal(expected: 5, data.Count);
            Assert.Contains(data, d => d.DisplayOrder == 6);
            Assert.Contains(data, d => d.DisplayOrder == 7);
            Assert.Contains(data, d => d.DisplayOrder == 8);
            Assert.Contains(data, d => d.DisplayOrder == 9);
            Assert.Contains(data, d => d.DisplayOrder == 10);

            query.Page = 3;
            actual = await handler.Handle(query, CancellationToken.None);

            // third call
            Assert.Equal(expected: 15, actual.TotalItems);
            Assert.Equal(expected: 3, actual.CurrentPage);
            Assert.Equal(expected: 3, actual.EndPage);
            Assert.Equal(expected: 1, actual.StartPage);
            Assert.Equal(expected: 3, actual.TotalPages);
            Assert.Equal(expected: 5, actual.Data.Count());
            data = actual.Data.ToList();
            Assert.Equal(expected: 5, data.Count);
            Assert.Contains(data, d => d.DisplayOrder == 11);
            Assert.Contains(data, d => d.DisplayOrder == 12);
            Assert.Contains(data, d => d.DisplayOrder == 13);
            Assert.Contains(data, d => d.DisplayOrder == 14);
            Assert.Contains(data, d => d.DisplayOrder == 15);

            query.Page = 4;
            actual = await handler.Handle(query, CancellationToken.None);
            // last call
            Assert.Equal(expected: 15, actual.TotalItems);
            Assert.Equal(expected: 4, actual.CurrentPage);
            Assert.Equal(expected: 3, actual.EndPage);
            Assert.Equal(expected: 1, actual.StartPage);
            Assert.Equal(expected: 3, actual.TotalPages);
            Assert.Empty(actual.Data);
        }

        [Fact]
        public void Validator()
        {
            var validator = new SearchTodoListsQuery.Validator();
            validator.ShouldHaveValidationErrorFor(s => s.Page, value: 0);
            validator.ShouldHaveValidationErrorFor(s => s.Page, value: -1);
            validator.ShouldNotHaveValidationErrorFor(s => s.Page, value: 1);

            validator.ShouldHaveValidationErrorFor(s => s.PageSize, value: -1);
            validator.ShouldHaveValidationErrorFor(s => s.PageSize, value: 4);
            validator.ShouldNotHaveValidationErrorFor(s => s.PageSize, value: 5);
        }
    }
}
