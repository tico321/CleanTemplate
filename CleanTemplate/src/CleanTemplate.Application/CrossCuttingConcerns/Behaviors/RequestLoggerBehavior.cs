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
        private readonly ICurrentUserService currentUserService;
        private readonly ILoggerAdapter<TRequest> logger;

        public RequestLoggerBehavior(ILoggerAdapter<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId ?? string.Empty;
            var userName = currentUserService.UserName ?? string.Empty;
            try
            {
                object requestLog = GetLogContent(request);
                logger.LogInformation(
                    "Request: {@Request} {@UserId} {@User} {@Data}",
                    requestName, userId, userName, requestLog);
                var response = await next();
                var responseLog = GetLogContent(response);
                logger.LogInformation(
                    "Response: {@Status} {@Request} {@UserId} {@User} {@Data}",
                    "Ok", requestName, userId, userName, responseLog);

                return response;
            }
            catch (AppException ex)
            {
                logger.LogInformation(
                    "Response: {@Status} {@Request} {@UserId} {@User} @{Failure}",
                    "ApplicationError", requestName, userId, userName, ex.GetFormattedMessage());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Response: {@Status} {@Request} {@UserId} {@User}",
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