using System;
using System.Net;
using System.Threading.Tasks;
using Common.Extensions;
using LoggerService.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Service.Exceptions;

namespace NetCore3Test.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                if (next != null)
                    await next(httpContext).ConfigureAwait(false);
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            if (exception is ArgumentException)
                code = HttpStatusCode.BadRequest;
            if (exception is ServiceInputException)
                code = HttpStatusCode.BadRequest;
            //if (exception is DomainException)
            //    code = HttpStatusCode.BadRequest;
            if (exception is NotFoundException)
                code = HttpStatusCode.NotFound;

            var friendlyErrorMessage = exception.Message;
            var fullErrorDetails = exception.GetErrorDetails();
            var errors = new string[1];

            if (code == HttpStatusCode.NotFound || code == HttpStatusCode.BadRequest)
            {
                errors[0] = $"{friendlyErrorMessage}.";
            }
            else
            {
                errors[0] = $"{friendlyErrorMessage}. View system logs for more details and/or ask an administrator about this error (ErrorType = {exception.GetType()}).";
                _logger.LogError(fullErrorDetails);
            }

            var result = JsonConvert.SerializeObject(new { error = errors });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result).ConfigureAwait(false);
        }
    }
}
