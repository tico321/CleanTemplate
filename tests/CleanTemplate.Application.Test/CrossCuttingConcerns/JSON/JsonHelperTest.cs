using System.Collections.Generic;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;
using CleanTemplate.Domain.Common;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.JSON
{
    public class JsonHelperTest
    {
        private class TestEntity : Entity<int>
        {
            public string Description { get; set; }
        }

        private class TestEnumeration : Enumeration
        {
            public static readonly TestEnumeration EnumA = new TestEnumeration(id: 1, "EnumA");

            private TestEnumeration(int id, string name) : base(id, name)
            {
            }
        }

        private class TestValueObject : ValueObject
        {
            public string Description { get; set; }

            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return Description;
            }
        }

        [Fact]
        public void ConvertsObjectToJson_WithPretty()
        {
            var obj = new TestEntity { Description = "description" };

            var actual = obj.ToJson(pretty: true);

            Assert.Equal("{\r\n  \"Description\": \"description\",\r\n  \"Id\": 0\r\n}", actual);
        }

        [Fact]
        public void ConvertsObjToJson_Default()
        {
            var tests = new List<(object, string)>
            {
                ("string value", "\"string value\""),
                (new TestEntity {Description = "description"}, "{\"Description\":\"description\",\"Id\":0}"),
                (TestEnumeration.EnumA, "{\"Name\":\"EnumA\",\"Id\":1}"),
                (new TestValueObject {Description = "description"}, "{\"Description\":\"description\"}")
            };

            foreach (var test in tests)
            {
                var (obj, expectedJson) = test;

                var actual = obj.ToJson();

                Assert.Equal(expectedJson, actual);
            }
        }
    }
}
