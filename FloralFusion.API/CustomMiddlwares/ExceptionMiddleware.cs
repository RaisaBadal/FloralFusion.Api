﻿using System.Net;

namespace FloralFusion.API.CustomMiddlwares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate request; //akontrolebs yvela shemosul da gasul request
        private readonly ILogger<ExceptionMiddleware> log;
        public ExceptionMiddleware(RequestDelegate del, ILogger<ExceptionMiddleware> log)
        {
            this.request = del;
            this.log = log;
        }

        #region InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await request(context);

                }
                catch (Exception ex)
                {
                    log.LogError($"Something went wrong: {ex}");
                    await HandleError(context, ex);
                }
                if (context.Response.StatusCode == 404)
                {
                    await HandleError(context, new Exception("No page found, Need  authorize"));
                    log.LogError($"No page found, Need  authorize");
                }
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        #endregion

        #region HandleError
        private Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                Detailed = ex.Source
            };
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
        #endregion
    }
}