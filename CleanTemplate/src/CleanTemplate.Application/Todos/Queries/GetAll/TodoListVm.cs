using System.Collections.Generic;

namespace CleanTemplate.Application.Todos.Queries.GetAll
{
    public class TodoListVm
    {
        public IEnumerable<TodoVm> Todos { get; set; }
    }
}
