using System.Collections.Generic;
using System.Linq;

namespace CleanTemplate.Domain.Common
{
    // learn more at https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, objB: null) ^ ReferenceEquals(otherValues.Current, objB: null))
                {
                    return false;
                }

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, objB: null) ^ ReferenceEquals(right, objB: null))
            {
                return false;
            }

            return ReferenceEquals(left, objB: null) || left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
