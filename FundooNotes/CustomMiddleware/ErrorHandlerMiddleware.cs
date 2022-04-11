using FundooNotes.CustomException;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FundooNotes.ErrorHandleMiddleware
{
    /// <summary>
    /// Created The Class To Use Global Error Hnadler Middleware To Catch All 
    /// Exceptions Thrown By The Api In A Single Place
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Method To Send Asynchronus Http Exceptions
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case CustomAppException e:
                        //For Custom Application Error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        //For Not Found Error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException e:
                        //For Unauthorized Error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        //For Unhandled Error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { success = false, message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
