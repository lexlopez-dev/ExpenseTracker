using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Core;
using ExpenseTracker.Filters.CategoryFilters;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return Ok(await _unitOfWork.Categories.GetAll());
        }

        // GET: api/Category/5
        [HttpGet("id/{id}")]
        [ServiceFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/Category/Expense
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesByType(string type)
        {
            var categories = await _unitOfWork.Categories.GetCategoriesByType(type);

            if (categories == null) return NotFound();

            return Ok(categories);
        }

        // PATCH: api/Category/5
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        [Category_ValidateUpdateCategoryFilter]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            //if (id != category.CategoryId)
            //{
            //    return BadRequest();
            //}

            await _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            await _unitOfWork.Categories.Add(category);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("CreateCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            await _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
