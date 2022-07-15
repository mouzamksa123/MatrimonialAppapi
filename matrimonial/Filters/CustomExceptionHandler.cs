using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace matrimonial.Filters
{
    public class CustomExceptionHandler : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        public CustomExceptionHandler(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            string errorMessage = context.Exception.Message;
            string stackTrace = context.Exception.StackTrace;
            Exception detailException = context.Exception;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";
            if (_env.IsProduction())
            {
                errorMessage = "Error occurred. Kindly contact administrator.";
                detailException = null;
                stackTrace = null;
            }
            var result = JsonConvert.SerializeObject(
                new
                {
                    message = errorMessage,
                    iserror = true,
                    detailException = detailException,
                    stackTrace = stackTrace
                });
            response.ContentLength = result.Length;
            response.WriteAsync(result);
        }
    }
}
