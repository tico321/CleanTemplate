using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Behaviors;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using CleanTemplate.Application.Test.TestHelpers;
using FakeItEasy;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Behaviors
{
    public class RequestLoggerBehaviorTest
    {
        public class SampleRequest
        {
            public string RequestData { get; set; }
        }

        public class SampleResponse
        {
            public string ResponseData { get; set; }
        }

        public class SampleRequestCustomLogging : ICustomLogging
        {
            public string ToLog()
            {
                return nameof(SampleRequestCustomLogging);
            }
        }

        public class SampleResponseCustomLogging : ICustomLogging
        {
            public string ToLog()
            {
                return nameof(SampleResponseCustomLogging);
            }
        }

        [Fact]
        public async Task Handle_Success()
        {
            var logger = A.Fake<ILoggerAdapter<RequestLoggerBehavior<SampleRequest, SampleResponse>>>();
            var userService = new FakeUserService();
            var sut = new RequestLoggerBehavior<SampleRequest, SampleResponse>(logger, userService);
            var request = new SampleRequest { RequestData = "request data" };
            var response = new SampleResponse { ResponseData = "response data" };
            var capturedCalls = new List<(string, object[])>();
            A
                .CallTo(() => logger.LogInformation(A<string>._, A<object[]>._))
                .Invokes((string msg, object[] args) => capturedCalls.Add((msg, args)));

            var actual = await sut.Handle(
                request,
                CancellationToken.None,
                () => Task.FromResult(response));

            Assert.Equal(response, actual); // It returns the correct result
            Assert.Equal(2, capturedCalls.Count);
            // Logs the request
            var (requestMsg, requestArgs) = capturedCalls[0];
            Assert.Equal("Processed request {@Request} for user {@UserId}-{@User} with data {@Data}", requestMsg);
            Assert.Equal(nameof(SampleRequest), requestArgs[0]);
            Assert.Equal(userService.UserId, requestArgs[1]); //userId
            Assert.Equal(userService.UserName, requestArgs[2]); //userName
            Assert.Equal(request, requestArgs[3]);
            // Logs the response
            var (responseMsg, responseArgs) = capturedCalls[1];
            Assert.Equal("Result {@Status} for request {@Request}: {@Data}", responseMsg);
            Assert.Equal("Ok", responseArgs[0]);
            Assert.Equal(nameof(SampleRequest), responseArgs[1]);
            Assert.Equal(response, responseArgs[2]); //data
        }

        [Fact]
        public async Task Handle_UnExpectedException_LogsError()
        {
            var logger = A.Fake<ILoggerAdapter<RequestLoggerBehavior<SampleRequest, SampleResponse>>>();
            var userService = new FakeUserService();
            var sut = new RequestLoggerBehavior<SampleRequest, SampleResponse>(logger, userService);
            var request = new SampleRequest { RequestData = "request data" };
            var capturedLogCalls = new List<(string, object[])>();
            A
                .CallTo(() => logger.LogInformation(A<string>._, A<object[]>._))
                .Invokes((string msg, object[] args) => capturedLogCalls.Add((msg, args)));
            var capturedErrorCalls = new List<(Exception, string, object[])>();
            A
                .CallTo(() => logger.LogError(A<Exception>._, A<string>._, A<object[]>._))
                .Invokes((Exception e, string msg, object[] args) => capturedErrorCalls.Add((e, msg, args)));
            try
            {
                await sut.Handle(
                    request,
                    CancellationToken.None,
                    () => throw new Exception("Exception Message"));
                Assert.False(true, "It should bubble up the exception");
            }
            catch (Exception e)
            {
                Assert.Single(capturedLogCalls);
                // Logs the request
                var (requestMsg, requestArgs) = capturedLogCalls[0];
                Assert.Equal("Processed request {@Request} for user {@UserId}-{@User} with data {@Data}", requestMsg);
                Assert.Equal(nameof(SampleRequest), requestArgs[0]);
                Assert.Equal(userService.UserId, requestArgs[1]); //userId
                Assert.Equal(userService.UserName, requestArgs[2]); //userName
                Assert.Equal(request, requestArgs[3]);
                // Logs the Error
                Assert.Single(capturedErrorCalls);
                var (_, responseMsg, responseArgs) = capturedErrorCalls[0];
                Assert.Equal("Exception Message", e.Message);
                Assert.Equal("Result {@Status} for request {@Request}: @{Failure} }", responseMsg);
                Assert.Equal("UnexpectedError", responseArgs[0]);
                Assert.Equal(nameof(SampleRequest), responseArgs[1]);
                Assert.Equal("Exception Message", responseArgs[2]);
            }
        }

        [Fact]
        public async Task Handle_WithApplicationException_LogsOnlyInformation()
        {
            var logger = A.Fake<ILoggerAdapter<RequestLoggerBehavior<SampleRequest, SampleResponse>>>();
            var userService = new FakeUserService();
            var sut = new RequestLoggerBehavior<SampleRequest, SampleResponse>(logger, userService);
            var request = new SampleRequest { RequestData = "request data" };
            var capturedCalls = new List<(string, object[])>();
            A
                .CallTo(() => logger.LogInformation(A<string>._, A<object[]>._))
                .Invokes((string msg, object[] args) => capturedCalls.Add((msg, args)));

            try
            {
                await sut.Handle(
                    request,
                    CancellationToken.None,
                    () => throw new AppException("AppException Message"));
                Assert.False(true, "It should bubble up the exception");
            }
            catch (AppException)
            {
                Assert.Equal(2, capturedCalls.Count);
                // Logs the request
                var (requestMsg, requestArgs) = capturedCalls[0];
                Assert.Equal("Processed request {@Request} for user {@UserId}-{@User} with data {@Data}", requestMsg);
                Assert.Equal(nameof(SampleRequest), requestArgs[0]);
                Assert.Equal(userService.UserId, requestArgs[1]); //userId
                Assert.Equal(userService.UserName, requestArgs[2]); //userName
                Assert.Equal(request, requestArgs[3]);
                // Error is logged as information because this is an expected AppException, most likely a bad request
                var (responseMsg, responseArgs) = capturedCalls[1];
                Assert.Equal("Result {@Status} for request {@Request}: @{Failure} }", responseMsg);
                Assert.Equal("ApplicationError", responseArgs[0]);
                Assert.Equal(nameof(SampleRequest), responseArgs[1]);
                Assert.Equal("AppException Message", responseArgs[2]);
            }
        }

        [Fact]
        public async Task Handle_WithCustomLogging_Success()
        {
            var logger =
                A.Fake<ILoggerAdapter<RequestLoggerBehavior<SampleRequestCustomLogging, SampleResponseCustomLogging>
                >>();
            var userService = new FakeUserService();
            var sut = new RequestLoggerBehavior<SampleRequestCustomLogging, SampleResponseCustomLogging>(
                logger,
                userService);
            var request = new SampleRequestCustomLogging();
            var response = new SampleResponseCustomLogging();
            var capturedCalls = new List<(string, object[])>();
            A
                .CallTo(() => logger.LogInformation(A<string>._, A<object[]>._))
                .Invokes((string msg, object[] args) => capturedCalls.Add((msg, args)));

            var actual = await sut.Handle(
                request,
                CancellationToken.None,
                () => Task.FromResult(response));

            Assert.Equal(response, actual); // It returns the correct result
            Assert.Equal(2, capturedCalls.Count);
            // Logs the request
            var (requestMsg, requestArgs) = capturedCalls[0];
            Assert.Equal("Processed request {@Request} for user {@UserId}-{@User} with data {@Data}", requestMsg);
            Assert.Equal(nameof(SampleRequestCustomLogging), requestArgs[0]);
            Assert.Equal(userService.UserId, requestArgs[1]); //userId
            Assert.Equal(userService.UserName, requestArgs[2]); //userName
            Assert.Equal(request.ToLog(), requestArgs[3]);
            // Logs the response
            var (responseMsg, responseArgs) = capturedCalls[1];
            Assert.Equal("Result {@Status} for request {@Request}: {@Data}", responseMsg);
            Assert.Equal("Ok", responseArgs[0]);
            Assert.Equal(nameof(SampleRequestCustomLogging), responseArgs[1]);
            Assert.Equal(response.ToLog(), responseArgs[2]);
        }

        [Fact]
        public async Task Handle_WithNoUser_Success()
        {
            var logger = A.Fake<ILoggerAdapter<RequestLoggerBehavior<SampleRequest, SampleResponse>>>();
            var userService = A.Fake<ICurrentUserService>();
            var sut = new RequestLoggerBehavior<SampleRequest, SampleResponse>(logger, userService);
            var request = new SampleRequest { RequestData = "request data" };
            var response = new SampleResponse { ResponseData = "response data" };
            var capturedCalls = new List<(string, object[])>();
            A
                .CallTo(() => logger.LogInformation(A<string>._, A<object[]>._))
                .Invokes((string msg, object[] args) => capturedCalls.Add((msg, args)));

            var actual = await sut.Handle(
                request,
                CancellationToken.None,
                () => Task.FromResult(response));

            Assert.Equal(response, actual); // It returns the correct result
            Assert.Equal(2, capturedCalls.Count);
            // Logs the request
            var (requestMsg, requestArgs) = capturedCalls[0];
            Assert.Equal("Processed request {@Request} for user {@UserId}-{@User} with data {@Data}", requestMsg);
            Assert.Equal(nameof(SampleRequest), requestArgs[0]);
            Assert.Equal(string.Empty, requestArgs[1]); //userId
            Assert.Equal(string.Empty, requestArgs[2]); //userName
            Assert.Equal(request, requestArgs[3]);
            // Logs the response
            var (responseMsg, responseArgs) = capturedCalls[1];
            Assert.Equal("Result {@Status} for request {@Request}: {@Data}", responseMsg);
            Assert.Equal("Ok", responseArgs[0]);
            Assert.Equal(nameof(SampleRequest), responseArgs[1]);
            Assert.Equal(response, responseArgs[2]);
        }
    }
}
