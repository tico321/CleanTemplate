using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Domain.Todos;

namespace CleanTemplate.Application.Todos.Queries
{
    public class TodoVm : IMapFrom<TodoItem>
    {
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
