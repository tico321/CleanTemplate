using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Model;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.QueryObjects
{
    public static class TodoListQueryObjects
    {
        public static Task<int> CountTodoLists(
            this IQueryable<TodoList> todoLists,
            string userId,
            CancellationToken token)
        {
            return todoLists
                .Where(l => l.UserId == userId)
                .CountAsync(token);
        }

        public static Task<TodoList> GetTodoListById(
            this IQueryable<TodoList> todoLists,
            int listId,
            CancellationToken token)
        {
            return todoLists
                .Include(t => t.Todos)
                .FirstOrDefaultAsync(l => l.Id == listId, token);
        }
    }
}
