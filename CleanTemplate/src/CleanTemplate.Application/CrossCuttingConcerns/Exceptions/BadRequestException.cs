using System.Collections.Generic;
using System.Linq;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;
using FluentValidation.Results;

namespace CleanTemplate.Application.CrossCuttingConcerns.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException() : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public BadRequestException(IEnumerable<ValidationFailure> failures) : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }

        public override string GetFormattedMessage()
        {
            return Failures.ToJson();
        }
    }
}
