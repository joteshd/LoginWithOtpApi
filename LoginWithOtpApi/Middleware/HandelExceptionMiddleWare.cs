using LoginWithOtpApi.UiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Utilities;
using System.Net;
using System.Text.Json;

namespace LoginWithOtpApi.Middleware
{
    public class HandelExceptionMiddleWare
    {
        readonly RequestDelegate _delegate;
        public HandelExceptionMiddleWare(RequestDelegate requestDelegate)
        {
            _delegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _delegate(httpContext);
            }
            catch (Exception ex)
            {
              await  HandleExceptionAsync(httpContext, ex);
            }
        
        }

        private static Task HandleExceptionAsync(HttpContext httpContext,Exception exception)
        {
          var exceptionHandelingCOnfig = httpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("ExceptionHandelingCOnfi");
            httpContext.Response.ContentType = "application/json";
            var response = httpContext.Response;
            bool isDevEnvironmetn = exceptionHandelingCOnfig["isDevEnvironment"].Equals("True", StringComparison.OrdinalIgnoreCase);

            var errorResponse = new ErrorResponseModel();
            errorResponse.isDevEnvironment = isDevEnvironmetn;

            if (errorResponse.isDevEnvironment)
            {
                switch (exception)
                {
                    case SqlException sqlException:
                    case ApplicationException applicationException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.ErrorMessage = "Something Went Wrong, Please try again after some time";
                        break;
                    case Exception ex:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.ErrorMessage = exception.Message;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.ErrorMessage = "Something Went Wrong, Please try again after some time";
                        break;
                }
            }
            var result = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(result);

        }

    }
}
