using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CleanTemplate.SharedKernel.Common
{
    // learn more at https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
    public abstract class Enumeration : IComparable
    {
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; }

        public int Id { get; }

        public int CompareTo(object? other)
        {
            if (other == null)
            {
                return -1;
            }

            return Id.CompareTo(((Enumeration)other).Id);
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields =
                typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Enumeration otherValue))
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        public static bool IsValidName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(v => v.Name == displayName);
            return matchingItem != null;
        }

        private static T Parse<T, TValue>(TValue value, string description, Func<T, bool> predicate)
            where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            return matchingItem ??
                   throw new InvalidOperationException($"'{value?.ToString() ?? "null"}' is not a valid {description} in {typeof(T)}");
        }
    }
}
