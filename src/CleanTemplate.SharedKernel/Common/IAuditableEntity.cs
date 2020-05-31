using System;

namespace CleanTemplate.SharedKernel.Common
{
    // based on https://github.com/JasonGT/CleanArchitecture/blob/master/src/Domain/Common/AuditableEntity.cs
    public interface IAuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
