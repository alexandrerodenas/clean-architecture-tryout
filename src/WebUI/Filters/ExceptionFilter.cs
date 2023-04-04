using System.Net;
using Application;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        switch (context.Exception)
        {
            case UserNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case IllegalCurrencyException:
            case DuplicatedExpenseException:
            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                break;
        }

        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = (int)statusCode;
        context.Result = new JsonResult(new
        {
            error = new[] { context.Exception.Message }
        });
    }
}
