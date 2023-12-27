using System;
using ExpenseTracker.Core;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters.ExceptionFilters
{
	public class Category_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        public Category_HandleUpdateExceptionsFilterAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strCategoryId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strCategoryId, out int categoryId))
            {
                if (!_unitOfWork.Categories.CategoryExists(categoryId))
                {
                    context.ModelState.AddModelError("CategoryId", "Category no longer exists.");
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

