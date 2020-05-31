using System;
using System.Linq;
using CleanTemplate.SharedKernel.Common;
using Xunit;

namespace CleanTemplate.SharedKernel.Test.Common
{
    public class EnumerationTest
    {
        public class CustomEnumeration : Enumeration
        {
            public static CustomEnumeration EnumA = new CustomEnumeration(id: 1, "EnumA");
            public static CustomEnumeration EnumB = new CustomEnumeration(id: 2, "EnumB");

            private CustomEnumeration(int id, string name) : base(id, name)
            {
            }
        }

        public class CustomEnumerationB : Enumeration
        {
            public static CustomEnumerationB EnumA = new CustomEnumerationB(id: 1, "EnumA");
            public static CustomEnumerationB EnumB = new CustomEnumerationB(id: 2, "EnumB");

            private CustomEnumerationB(int id, string name) : base(id, name)
            {
            }
        }

        public class CustomClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void AbsoluteDifference()
        {
            var enumA = CustomEnumeration.EnumA;
            var enumB = CustomEnumeration.EnumB;

            Assert.Equal(expected: 1, Enumeration.AbsoluteDifference(enumA, enumB));
            Assert.Equal(expected: 1, Enumeration.AbsoluteDifference(enumB, enumA));
        }

        [Fact]
        public void CompareTo()
        {
            var enumA = CustomEnumeration.EnumA;
            var enumA2 = CustomEnumeration.EnumA;
            var enumB = CustomEnumeration.EnumB;

            Assert.True(enumA.Equals(enumA2));
            Assert.True(enumA.CompareTo(enumA2) == 0);
            Assert.True(enumA.CompareTo(enumB) == -1);
            Assert.True(enumB.CompareTo(enumA) == 1);
            Assert.True(enumB.CompareTo(null) == -1);

            var enumBA = CustomEnumerationB.EnumA;
            Assert.False(enumA.Equals(enumBA));
            var classA = new CustomClass { Id = CustomEnumeration.EnumA.Id, Name = CustomEnumeration.EnumA.Name };
            Assert.False(enumA.Equals(classA));
        }

        [Fact]
        public void FactoryMethods()
        {
            var fromDisplayName = Enumeration.FromDisplayName<CustomEnumeration>("EnumA");
            Assert.Equal(CustomEnumeration.EnumA, fromDisplayName);

            var fromValue = Enumeration.FromValue<CustomEnumeration>(value: 1);
            Assert.Equal(CustomEnumeration.EnumA, fromValue);

            try
            {
                Enumeration.FromDisplayName<CustomEnumeration>("Invalid");
                Assert.True(condition: false, "Should throw exception.");
            }
            catch (InvalidOperationException e)
            {
                Assert.NotNull(e);
            }
        }

        [Fact]
        public void GetAll_ReturnsAllEnums()
        {
            var enums = Enumeration.GetAll<CustomEnumeration>().ToList();

            Assert.Equal(expected: 2, enums.Count);
            Assert.Contains(enums, e => e.Id == CustomEnumeration.EnumA.Id);
            Assert.Contains(enums, e => e.Id == CustomEnumeration.EnumB.Id);
        }

        [Fact]
        public void GetHashCode_ReturnsUniqueValues()
        {
            var enumA = CustomEnumeration.EnumA;
            var enumB = CustomEnumeration.EnumB;

            Assert.NotEqual(enumA.GetHashCode(), enumB.GetHashCode());
        }

        [Fact]
        public void ToString_ReturnsTheName()
        {
            Assert.Equal("EnumA", CustomEnumeration.EnumA.ToString());
        }

        [Fact]
        public void IsValidName()
        {
            Assert.True(Enumeration.IsValidName<CustomEnumeration>(CustomEnumeration.EnumA.Name));
            Assert.False(Enumeration.IsValidName<CustomEnumeration>("invalid"));
        }
    }
}
