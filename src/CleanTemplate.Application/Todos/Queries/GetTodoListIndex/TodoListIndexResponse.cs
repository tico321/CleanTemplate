using System.Collections.Generic;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListIndex
{
    public class TodoListIndexResponse
    {
        public IEnumerable<SimplifiedTodoListVm> Todos { get; set; }
    }
}
