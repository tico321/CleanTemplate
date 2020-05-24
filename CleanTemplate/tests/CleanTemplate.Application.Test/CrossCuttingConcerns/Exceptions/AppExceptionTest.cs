using System.Collections.Generic;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Exceptions
{
    public class AppExceptionTest
    {
        [Fact]
        public void GetFormattedMessage_ListConstructor()
        {
            var ex = new AppException(new List<string> {"m1", "m2"});

            var actual = ex.GetFormattedMessage();

            Assert.Equal("[\"m1\",\"m2\"]", actual);
        }

        [Fact]
        public void GetFormattedMessage_StringConstructor()
        {
            var ex = new AppException("some message");

            var actual = ex.GetFormattedMessage();

            Assert.Equal("some message", actual);
        }
    }
}
