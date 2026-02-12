using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleWPFWork.Host.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            object result;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = new
                    {
                        error = "Doğrulama hatası",
                        details = validationException.Errors.Select(e => new
                        {
                            property = e.PropertyName,
                            message = e.ErrorMessage
                        }).ToList()
                    };
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = new
                    {
                        error = "Kayıt bulunamadı",
                        details = exception.Message
                    };
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = new
                    {
                        error = "Yetkisiz erişim",
                        details = exception.Message
                    };
                    break;

                default:
                    result = new
                    {
                        error = "Sunucu hatası",
                        details = exception.Message
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}