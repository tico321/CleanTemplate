using CleanTemplate.Domain.Common;

namespace CleanTemplate.Application.Todos.Model
{
    public class TodoItemState : Enumeration
    {
        public static readonly TodoItemState Pending = new TodoItemState(id: 1, nameof(Pending));
        public static readonly TodoItemState Completed = new TodoItemState(id: 2, nameof(Completed));

        private TodoItemState(int id, string name) : base(id, name)
        {
        }
    }
}
