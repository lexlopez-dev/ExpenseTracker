using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Repositories
{
	public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
	{
		public TransactionRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
		{
		}

        public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int id)
        {
            try
            {
                return await _context.Transactions
                    .Include(t => t.Category)
                    .Where(x => x.CategoryId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public override async Task<IEnumerable<Transaction>> GetAll()
        {
            try
            {
                return await _context.Transactions.Include(t => t.Category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public override async Task<Transaction?> GetById(int id)
        {
            try
            {
                return await _context.Transactions
                    .Include(t => t.Category)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Transactionid == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

