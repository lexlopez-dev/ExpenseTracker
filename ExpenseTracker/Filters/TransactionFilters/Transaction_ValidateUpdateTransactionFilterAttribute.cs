using System;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters.TransactionFilters
{
	public class Transaction_ValidateUpdateTransactionFilterAttribute : ActionFilterAttribute
	{
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            var transaction = context.ActionArguments["transaction"] as Transaction;

            if (id.HasValue && transaction != null && id != transaction.TransactionId)
            {
                context.ModelState.AddModelError("TransactionId", "TransactionId is not the same as id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}

