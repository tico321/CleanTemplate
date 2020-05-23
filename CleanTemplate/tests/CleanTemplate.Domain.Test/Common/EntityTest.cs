using CleanTemplate.Domain.Common;
using Xunit;

namespace CleanTemplate.Domain.Test.Common
{
    public class EntityTest
    {
        [Fact]
        public void Comparison_IsDoneWithIds()
        {
            var entityIntA = new EntityInt {Id = 1, Value = "Value"};
            var entityIntB = new EntityInt {Id = 1, Value = "Other value"};
            var entityIntC = new EntityInt {Id = 2, Value = "Value"};

            Assert.True(entityIntA.Equals(entityIntB), "They should be equal as the id is the same");
            Assert.True(entityIntA == entityIntB, "They should be equal as the id is the same");
            Assert.True(entityIntA == entityIntA);
            Assert.True(entityIntA.Equals(entityIntA));

            Assert.False(entityIntA.Equals(entityIntC), "It should be false as the id is different.");
            Assert.False(entityIntA == entityIntC, "It should be false as the id is different.");
            Assert.True(entityIntA != entityIntC, "It should be true as the id is different.");
            Assert.NotEqual(entityIntA.GetHashCode(), entityIntC.GetHashCode());

            var otherEntityIntA = new OtherEntityInt {Id = 1, Value = "Value"};
            Assert.False(otherEntityIntA.Equals(entityIntA), "They have the same values but are different types");
            Assert.False(otherEntityIntA == entityIntA, "They have the same values but are different types");
            Assert.True(otherEntityIntA != entityIntA, "They have the same values but are different types");
        }

        [Fact]
        public void Comparison_WithStrings()
        {
            var entityIntA = new EntityString {Id = "1", Value = "Value"};
            var entityIntB = new EntityString {Id = "1", Value = "Other value"};
            var entityIntC = new EntityString {Id = "2", Value = "Value"};

            Assert.True(entityIntA.Equals(entityIntB), "They should be equal as the id is the same");
            Assert.True(entityIntA == entityIntB, "They should be equal as the id is the same");
            Assert.True(entityIntA == entityIntA);
            Assert.True(entityIntA.Equals(entityIntA));

            Assert.False(entityIntA.Equals(entityIntC), "It should be false as the id is different.");
            Assert.False(entityIntA == entityIntC, "It should be false as the id is different.");
            Assert.True(entityIntA != entityIntC, "It should be true as the id is different.");
        }
    }

    public class EntityInt : Entity<int>
    {
        public string Value { get; set; }
    }

    public class OtherEntityInt : Entity<int>
    {
        public string Value { get; set; }
    }

    public class EntityString : Entity<string>
    {
        public string Value { get; set; }
    }
}
