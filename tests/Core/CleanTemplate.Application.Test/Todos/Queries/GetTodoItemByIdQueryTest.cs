﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.GetTodoItemById;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetTodoItemByIdQueryTest : IClassFixture<RequestTestFixture>
    {
        public GetTodoItemByIdQueryTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task GetTodoItem()
        {
            var itemDescription = "todo1";
            var todos = new List<TodoList> { new TodoList("userId", "desc", 0).SequenceAddTodo(itemDescription) };
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var todoList = _fixture.Context.TodoLists.First();
            var query = new GetTodoItemByIdQuery { Id = todoList.Id, ItemId = todoList.Todos.First().Id };
            var handler = new GetTodoItemByIdQuery.Handler(_fixture.Context, _fixture.Mapper);

            var item = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(item);
            Assert.Equal(itemDescription, item.Description);
            Assert.Equal(TodoItemState.Pending.Name, item.State);
            Assert.Equal(1, item.DisplayOrder);
        }

        [Fact]
        public async Task GetTodoItem_ItemNotFound()
        {
            var todos = new List<TodoList> { new TodoList("userId", "desc2", 0) };
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var todoList = _fixture.Context.TodoLists.First();
            var query = new GetTodoItemByIdQuery { Id = todoList.Id, ItemId = 2 };
            var handler = new GetTodoItemByIdQuery.Handler(_fixture.Context, _fixture.Mapper);

            try
            {
                await handler.Handle(query, CancellationToken.None);
                Assert.False(true, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("2", e.Message);
            }
        }

        [Fact]
        public async Task GetTodoItem_ListNotFound()
        {
            var query = new GetTodoItemByIdQuery { Id = 10, ItemId = 2 };
            var handler = new GetTodoItemByIdQuery.Handler(_fixture.Context, _fixture.Mapper);

            try
            {
                await handler.Handle(query, CancellationToken.None);
                Assert.False(true, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("10", e.Message);
            }
        }
    }
}
