using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Model;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.QueryObjects
{
    public static class TodoItemQueries
    {
        public static Task<TodoItem> GetTodoItemById(
            this IQueryable<TodoList> todos,
            int todoListId,
            int todoItemId,
            CancellationToken cancellationToken)
        {
            return todos
                .Where(t => t.Id == todoListId)
                .SelectMany(t => t.Todos)
                .FirstOrDefaultAsync(t => t.Id == todoItemId, cancellationToken);
        }
    }
}
