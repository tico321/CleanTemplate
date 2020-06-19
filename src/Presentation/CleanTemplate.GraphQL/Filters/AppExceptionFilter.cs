using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using HotChocolate;

namespace CleanTemplate.GraphQL.Filters
{
    public class AppExceptionFilter : IErrorFilter
    {
        private readonly ILoggerAdapter<AppExceptionFilter> _logger;
        private readonly ICurrentUserService _userService;

        public AppExceptionFilter(ILoggerAdapter<AppExceptionFilter> logger, ICurrentUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IError OnError(IError error)
        {
            switch (error.Exception)
            {
                case BadRequestException br:
                    var failures = br.ToProblemDetails()
                        .ProblemDetails
                        .ToDictionary(kv => kv.Key, kv => kv.Value as object);
                    error = error
                        .WithMessage(br.Message)
                        .WithCode(((int)HttpStatusCode.BadRequest).ToString())
                        .WithExtensions(new ReadOnlyDictionary<string, object>(failures));
                    _logger.LogInformation(
                        "{ status: {@Status}, request: {@Request}, userId: {@UserId}, user: {@User}, error: @{Failure} }",
                        "ApplicationError",
                        "",
                        _userService.UserId,
                        _userService.UserName,
                        br.GetFormattedMessage());
                    break;
                case NotFoundException nf:
                    error = error.WithMessage(nf.Message);
                    _logger.LogInformation(
                        "{ status: {@Status}, request: {@Request}, userId: {@UserId}, user: {@User}, error: @{Failure} }",
                        "ApplicationError",
                        "",
                        _userService.UserId,
                        _userService.UserName,
                        nf.GetFormattedMessage());
                    Console.WriteLine("NotFoundException");
                    break;
                default:
                    _logger.LogError(
                        error.Exception,
                        "{ status: {@Status}, info: {@Request}, userId: {@UserId}, user: {@User} }",
                        "UnexpectedError",
                        error.Message,
                        _userService.UserId,
                        _userService.UserName);
                    Console.WriteLine("default");
                    break;
            }

            return error;
        }
    }
}
