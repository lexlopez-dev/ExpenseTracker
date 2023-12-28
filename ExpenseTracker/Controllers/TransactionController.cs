using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Core;
using ExpenseTracker.Filters.CategoryFilters;
using ExpenseTracker.Filters.TransactionFilters;
using ExpenseTracker.Filters.ExceptionFilters;
using ExpenseTracker.Filters.AuthFilters;

namespace ExpenseTracker.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtTokenAuthFilter]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            return Ok(await _unitOfWork.Transactions.GetAll());
        }

        // GET: api/Transaction/id/5
        [HttpGet("id/{id}")]
        [ServiceFilter(typeof(Transaction_ValidateTransactionIdFilterAttribute))]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetById(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET: api/Transaction/category/5
        [HttpGet("category/{id}")]
        [ServiceFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByCategoryId(int id)
        {
            var transactions = await _unitOfWork.Transactions.GetTransactionsByCategoryId(id);
            if (transactions == null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }

        // PATCH: api/Transaction/5
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(Transaction_ValidateTransactionIdFilterAttribute))]
        [Transaction_ValidateUpdateTransactionFilter]
        [ServiceFilter(typeof(Transaction_HandleUpdateExceptionsFilterAttribute))]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction transaction)
        {
            await _unitOfWork.Transactions.Update(transaction);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // POST: api/Transaction
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
        {
            // Confirm the category exists
            var categoryExists = await _unitOfWork.Categories.GetById(transaction.CategoryId);
            if (categoryExists == null)
            {
                return NotFound();
            }

            await _unitOfWork.Transactions.Add(transaction);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("CreateTransaction", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(Transaction_ValidateTransactionIdFilterAttribute))]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            await _unitOfWork.Transactions.Delete(transaction);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
