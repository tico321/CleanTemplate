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
                    "Processed request {@Request} for user {@UserId}-{@User} with data {@Data}",
                    requestName,
                    userId,
                    userName,
                    requestLog);
                var response = await next();
                var responseLog = GetLogContent(response);
                _logger.LogInformation(
                    "Result {@Status} for request {@Request}: {@Data}",
                    "Ok",
                    requestName,
                    responseLog);

                return response;
            }
            catch (AppException ex)
            {
                _logger.LogInformation(
                    "Result {@Status} for request {@Request}: @{Failure} }",
                    "ApplicationError",
                    requestName,
                    ex.GetFormattedMessage());
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Result {@Status} for request {@Request}: @{Failure} }",
                    "UnexpectedError",
                    requestName,
                    ex.Message);
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
