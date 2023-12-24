using ExpenseTracker.Models;

namespace ExpenseTracker.Core
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		Task<IEnumerable<Category>> GetCategoriesByType(string type);
	}
}

