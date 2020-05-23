using System.Collections.Generic;
using CleanTemplate.Domain.Common;
using Xunit;

namespace CleanTemplate.Domain.Test.Common
{
    public class ValueObjectTest
    {
        [Fact]
        public void Comparison()
        {
            var address1 = new Address{ City = "Some city", Street = "Some street"};
            var address2 = new Address{ City = "Some city 2", Street = "Some street 2"};
            var address3 = new Address{ City = null, Street = "Some street 2"};
            var email = new Email{Address = "some@email.com"};

            Assert.NotEqual(address1, address2);
            Assert.NotEqual(address1, address3);
            Assert.False(address1.Equals(email));
            Assert.False(address1 == address2);
            Assert.False(address1 == null);
            Assert.NotEqual(address1.GetHashCode(), address2.GetHashCode());

            var address1Equivalent = new Address{ City = "Some city", Street = "Some street"};
            Assert.Equal(address1, address1Equivalent);
            Assert.True(address1 == address1Equivalent);
            Assert.False(address1 != address1Equivalent);

            var copy = address1.GetCopy();
            Assert.Equal(copy, address1);
            Assert.Equal(copy.GetHashCode(), address1.GetHashCode());
        }
    }

    public class Address : ValueObject
    {
        public string City { get; set; }
        public string Street { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return City;
            yield return Street;
        }
    }

    public class Email : ValueObject
    {
        public string Address { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
        }
    }
}
