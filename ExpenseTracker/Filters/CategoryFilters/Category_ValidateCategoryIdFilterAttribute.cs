using ExpenseTracker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters.CategoryFilters
{
	public class Category_ValidateCategoryIdFilterAttribute : ActionFilterAttribute, IActionFilter
	{
        private readonly IUnitOfWork _unitOfWork;

        public Category_ValidateCategoryIdFilterAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var categoryId = context.ActionArguments["id"] as int?;
            if (categoryId.HasValue)
            {
                if (categoryId.Value <= 0)
                {
                    context.ModelState.AddModelError("CategoryId", "CategoryId is invalid");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else if (!_unitOfWork.Categories.CategoryExists(categoryId.Value))
                {
                    context.ModelState.AddModelError("CategoryId", "Category doesn't exist");
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

