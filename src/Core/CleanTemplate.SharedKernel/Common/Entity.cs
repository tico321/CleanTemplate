using System;

namespace CleanTemplate.SharedKernel.Common
{
    // based on https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.Domain/SeedWork/Entity.cs
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; set; } = default!;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = (Entity<TId>)obj;

            return item.Id?.Equals(Id) ?? throw new InvalidOperationException();
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? throw new InvalidOperationException();
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !(left == right);
        }
    }
}
