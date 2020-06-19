using CleanTemplate.SharedKernel.Common;

namespace CleanTemplate.Application.Todos.Model
{
    public class TodoItemState : Enumeration
    {
        public static readonly TodoItemState Pending = new TodoItemState(1, nameof(Pending));
        public static readonly TodoItemState Completed = new TodoItemState(2, nameof(Completed));

        private TodoItemState(int id, string name) : base(id, name)
        {
        }
    }
}
