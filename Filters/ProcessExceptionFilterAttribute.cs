using BusinessEntity.Error;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace FacturacionApi.Filters
{
    public class ProcessExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception is CustomException)
            {
                ErrorResponse error = new ErrorResponse();
                error.message = actionExecutedContext.Exception.Message;

                var json = JsonConvert.SerializeObject(error, Formatting.Indented);

                var stringContent = new StringContent(json);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = stringContent
                };

                actionExecutedContext.Response = response;
                actionExecutedContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            if (actionExecutedContext.Exception is CustomExternalException)
            {
                ErrorResponse error = new ErrorResponse();
                error.message = actionExecutedContext.Exception.Message;

                var json = JsonConvert.SerializeObject(error, Formatting.Indented);

                var stringContent = new StringContent(json);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = stringContent
                };

                actionExecutedContext.Response = response;
                actionExecutedContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }
    }
}