using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanTemplate.API.Filters
{
    public class AppExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case BadRequestException br:
                    context.Result = new BadRequestObjectResult(br.ToProblemDetails());
                    context.ExceptionHandled = true;
                    break;
                case NotFoundException nf:
                    context.Result = new NotFoundObjectResult(nf.Key);
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
