using System;
using System.Collections.Generic;
using System.Linq;
using CleanTemplate.SharedKernel.Common;

namespace CleanTemplate.Application.Todos.Model
{
    public class TodoList : Entity<int>, IAuditableEntity
    {
        private List<TodoItem> _todos;

        public TodoList(string userId, string description, int displayOrder)
        {
            UserId = userId;
            Description = description;
            DisplayOrder = displayOrder;
            _todos = new List<TodoItem>();
        }

        public string UserId { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public IEnumerable<TodoItem> Todos => _todos;

        public TodoList SequenceAddTodo(string description)
        {
            AddTodo(description);
            return this;
        }

        public TodoItem AddTodo(string description)
        {
            var todoItem = new TodoItem(description, _todos.Count + 1)
            {
                TodoList = this,
                TodoListId = Id
            };
            _todos.Add(todoItem);
            return todoItem;
        }

        public bool RemoveTodo(int id)
        {
            var item = _todos.FirstOrDefault(t => t.Id == id);
            return _todos.Remove(item);
        }

        public void SortTodosByDisplayOrder()
        {
            _todos = _todos.OrderBy(t => t.DisplayOrder).ToList();
        }

        #region auditable

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
