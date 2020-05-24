using System;
using CleanTemplate.Domain.Common;

namespace CleanTemplate.Domain.Todos
{
    public class TodoItem : IAuditableEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }

        #region auditable

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
