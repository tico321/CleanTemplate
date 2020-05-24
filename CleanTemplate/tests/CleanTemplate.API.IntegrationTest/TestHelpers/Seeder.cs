using CleanTemplate.Application.Test.Todos;
using CleanTemplate.Infrastructure.Persistence;

namespace CleanTemplate.API.TestHelpers
{
    public class Seeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.TodoItems.AddRange(TodoSeeder.DefaultTodoItems);
            context.SaveChanges();
        }

        public static void Reset(ApplicationDbContext context)
        {
            context.TodoItems.RemoveRange(context.TodoItems);
        }
    }
}
