using System.Collections.Generic;

namespace CleanTemplate.Application.CrossCuttingConcerns.Exceptions
{
    public class ErrorDetails
    {
        public ErrorDetails(string message, IDictionary<string, string[]> problemDetails = null)
        {
            Message = message;
            ProblemDetails = problemDetails ?? new Dictionary<string, string[]>();
        }

        public string Message { get; }
        public IDictionary<string, string[]> ProblemDetails { get; }
    }
}
