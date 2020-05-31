using System.Collections.Generic;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using FluentValidation.Results;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Exceptions
{
    public class BadRequestExceptionTest
    {
        [Fact]
        public void GetFormattedMessage()
        {
            var ex = new BadRequestException(new List<ValidationFailure> { new ValidationFailure("data", "invalid") });

            var actual = ex.GetFormattedMessage();

            Assert.Equal("{\"data\":[\"invalid\"]}", actual);
        }

        [Fact]
        public void ToProblemDetails()
        {
            var ex = new BadRequestException(new List<ValidationFailure> { new ValidationFailure("data", "invalid") });

            var actual = ex.ToProblemDetails();

            Assert.Equal("One or more validation failures have occurred.", actual.Message);
            Assert.Equal(expected: 1, actual.ProblemDetails.Keys.Count);
        }
    }
}
