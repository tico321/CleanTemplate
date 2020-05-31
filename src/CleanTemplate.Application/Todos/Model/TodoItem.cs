using System;
using CleanTemplate.SharedKernel.Common;

namespace CleanTemplate.Application.Todos.Model
{
    public class TodoItem : Entity<int>, IAuditableEntity
    {
        public TodoItem(string description, int displayOrder)
        {
            Description = description;
            DisplayOrder = displayOrder;
            State = TodoItemState.Pending;
        }

        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public TodoItemState State { get; set; }
        public int TodoListId { get; set; }
        public TodoList TodoList { get; set; }

        #region auditable

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
