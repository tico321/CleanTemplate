using System;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using MediatR;

namespace CleanTemplate.Application.CrossCuttingConcerns.Behaviors
{
    public class RequestLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerAdapter<RequestLoggerBehavior<TRequest, TResponse>> _logger;

        public RequestLoggerBehavior(
            ILoggerAdapter<RequestLoggerBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var userName = _currentUserService.UserName ?? string.Empty;
            try
            {
                var requestLog = GetLogContent(request);
                _logger.LogInformation(
                    "{ request: {@Request}, userId: {@UserId}, user: {@User}, data: {@Data} }",
                    requestName, userId, userName, requestLog);
                var response = await next();
                var responseLog = GetLogContent(response);
                _logger.LogInformation(
                    "{ status: {@Status}, response: {@Request}, userId {@UserId}, user: {@User}, data: {@Data} }",
                    "Ok", requestName, userId, userName, responseLog);

                return response;
            }
            catch (AppException ex)
            {
                _logger.LogInformation(
                    "{ status: {@Status}, request: {@Request}, userId: {@UserId}, user: {@User}, error: @{Failure} }",
                    "ApplicationError", requestName, userId, userName, ex.GetFormattedMessage());
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "{ status: {@Status}, request: {@Request}, userId: {@UserId}, user: {@User} }",
                    "UnexpectedError", requestName, userId, userName);
                throw;
            }
        }

        private object GetLogContent(object request)
        {
            if (request is ICustomLogging customLog)
            {
                return customLog.ToLog();
            }

            return request;
        }
    }
}
