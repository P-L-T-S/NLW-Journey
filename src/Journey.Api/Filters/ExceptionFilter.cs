using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Journey.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is JourneyException)
        {
            var journeyException = (JourneyException)context.Exception;

            var response = new ResponseErrorsJson(journeyException.GetErrorList());

            context.HttpContext.Response.StatusCode = (int)journeyException.GetStatusCode();
            context.Result = new ObjectResult(response);
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // var response = new ResponseErrorsJson(new List<string> { "Erro desconhecido" });
            var list = new List<string> { context.Exception.Message };
            var response = new ResponseErrorsJson(list);

            context.Result = new ObjectResult(response);
        }
    }
}