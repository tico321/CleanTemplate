using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using HotChocolate;

namespace CleanTemplate.GraphQL.Filters
{
    public class AppExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            switch (error.Exception)
            {
                case BadRequestException br:
                    error.WithMessage(br.Message);
                    error.WithCode(((int)HttpStatusCode.BadRequest).ToString());
                    var failures = br.ToProblemDetails().ProblemDetails
                        .ToDictionary(kv => kv.Key, kv => kv.Value as object);
                    error.WithExtensions(new ReadOnlyDictionary<string, object>(failures));
                    break;
                case NotFoundException nf:
                    error.WithMessage(nf.Message);
                    break;
            }

            Console.WriteLine(error);
            Console.WriteLine(error.Exception);
            Console.WriteLine(error.Message);
            return error;
        }
    }
}
