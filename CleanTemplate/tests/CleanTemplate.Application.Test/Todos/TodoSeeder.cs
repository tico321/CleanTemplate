using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Domain.Todos;

namespace CleanTemplate.Application.Test.Todos
{
    public class TodoSeeder
    {
        public static readonly List<TodoItem> DefaultTodoItems = new List<TodoItem>
        {
            new TodoItem {Id = 1, Description = "Take out the trash"},
            new TodoItem {Id = 2, Description = "Finish my homework"}
        };

        public static Func<IApplicationDbContext, Task> GetSeeder(List<TodoItem> items)
        {
            return context =>
            {
                context.TodoItems.AddRange(items);
                return context.SaveChangesAsync(CancellationToken.None);
            };
        }
    }
}
