using System;
using ExpenseTracker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters.ExceptionFilters
{
	public class Transaction_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
	{
        private readonly IUnitOfWork _unitOfWork;

        public Transaction_HandleUpdateExceptionsFilterAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        } 

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strTransactionId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strTransactionId, out int transactionId))
            {
                if (!_unitOfWork.Transactions.TransactionExists(transactionId))
                {
                    context.ModelState.AddModelError("TransactionId", "Transaction no longer exists.");
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

