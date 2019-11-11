using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dalisama.ProjectSample.Host.Common.Middleware
{
    public class TraceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceLoggingMiddleware> _logger;

        public TraceLoggingMiddleware(RequestDelegate next, ILogger<TraceLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //check if the request has already a correlation id
            context.Request.Headers.TryGetValue("X-Request-Trace", out StringValues traceId);
            // if no correlation id is found, generate a new one
            if (traceId == default(StringValues)) traceId = Guid.NewGuid().ToString();
            // add the correlation id to the response
            context.Response.Headers.Add("X-Request-Trace", traceId);
            // Adding the correlation id to all log related to the current request
            using (_logger.BeginScope(new Dictionary<string, object> { ["X-Request-Trace"] = traceId.ToString() }))
            {

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {

                    await HandleExceptionAsync(context, traceId, ex);
                }
            }
        }
        private Task HandleExceptionAsync(HttpContext context, string traceId, Exception exception)
        {

            var error = new
            {
                Id = traceId.ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API.  Please use the id and contact our " +
                        "support team if the problem persists."
            };



            var innerExMessage = GetInnermostExceptionMessage(exception);

            _logger.LogError(exception, innerExMessage + " -- {ErrorId}.", error.Id);

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnermostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}

