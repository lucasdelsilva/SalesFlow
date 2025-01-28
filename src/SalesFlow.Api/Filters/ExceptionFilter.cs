using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SalesFlow.Communication.Response;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is SalesFlowException)
            HandleProjectExeption(context);
        else
            ThrowUnknownError(context);
    }

    private static void HandleProjectExeption(ExceptionContext context)
    {
        var exceptionSale = (SalesFlowException)context.Exception;
        var errorResponse = new ResponseErrorJson(exceptionSale!.GetErros());

        context.HttpContext.Response.StatusCode = exceptionSale.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        var message = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(message);
    }
}