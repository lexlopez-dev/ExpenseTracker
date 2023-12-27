using System;
using ExpenseTracker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters.TransactionFilters
{
	public class Transaction_ValidateTransactionIdFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public Transaction_ValidateTransactionIdFilterAttribute(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var transactionId = context.ActionArguments["id"] as int?;
            if (transactionId.HasValue)
            {
                if (transactionId.Value <= 0)
                {
                    context.ModelState.AddModelError("TransactionId", "TransactionId is invalid");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else if (!_unitOfWork.Transactions.TransactionExists(transactionId.Value))
                {
                    context.ModelState.AddModelError("TransactionId", "Transaction doesn't exist");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}

