using ExpenseTracker.Models;

namespace ExpenseTracker.Core
{
	public interface ITransactionRepository : IGenericRepository<Transaction>
	{
		Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int id);
	}
}

