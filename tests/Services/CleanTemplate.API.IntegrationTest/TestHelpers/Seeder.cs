using CleanTemplate.Application.Test.Todos;
using CleanTemplate.Infrastructure.Persistence;

namespace CleanTemplate.API.TestHelpers
{
    public class Seeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.TodoLists.AddRange(TodoSeeder.DefaultTodoLists);
            context.SaveChanges();
        }

        public static void Reset(ApplicationDbContext context)
        {
            context.TodoLists.RemoveRange(context.TodoLists);
            // Cascade delete is not supported in memory DB https://github.com/dotnet/efcore/issues/3924
            context.TodoItems.RemoveRange(context.TodoItems);
            context.SaveChanges();
        }
    }
}
