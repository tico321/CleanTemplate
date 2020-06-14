using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Model;

namespace CleanTemplate.Application.Test.Todos
{
    public class TodoSeeder
    {
        public static readonly List<TodoList> DefaultTodoLists = new List<TodoList>
        {
            new TodoList("0", "Personal", displayOrder: 2),
            new TodoList("1", "Work", displayOrder: 1)
                .SequenceAddTodo("Reply Juan Email")
                .SequenceAddTodo("Finish monthly report")
        };

        public static Func<IApplicationDbContext, Task> GetSeeder(List<TodoList> items)
        {
            return context =>
            {
                var todos = context.TodoLists.ToList();
                context.TodoLists.AddRange(items.Where(i => !todos.Contains(i)));
                return context.SaveChangesAsync(CancellationToken.None);
            };
        }
    }
}
