using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CashFlowException)
            {
                HandleProjectExeception(context);
            }
            else
            {
                ThrowUnkowError(context);
            }
        }

        private void HandleProjectExeception(ExceptionContext context)
        {
            if(context.Exception is ErrorOnValidationException)
            {
                var ex = (ErrorOnValidationException)context.Exception;
                var erroMessage = new ResponseErrosJson(ex.Errors);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(erroMessage);

            }
            else
            { //TODO: Temporario
                var erroMessage = new ResponseErrosJson(context.Exception.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(erroMessage);
            }
        }

        private void ThrowUnkowError(ExceptionContext context)
        {
            var erroMessage = new ResponseErrosJson("Unknown error");

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(erroMessage);

            
        }

    }
}
