using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Core;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByCategoryId(int categoryId)
        {
            // Confirm the category exists
            var categoryExists = await _unitOfWork.Categories.GetById(categoryId);
            if (categoryExists == null)
            {
                return NotFound();
            }

            var transactions = await _unitOfWork.Transactions.GetTransactionsByCategoryId(categoryId);
            if (transactions == null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }

        // PATCH: api/Transaction/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Transactionid)
            {
                return BadRequest();
            }

            var transactionExists = await _unitOfWork.Transactions.GetById(id);

            if (transactionExists == null) return NotFound();

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

            return CreatedAtAction("CreateTransaction", new { id = transaction.Transactionid }, transaction);
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
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
