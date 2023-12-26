using System;
namespace ExpenseTracker.Core
{
	public interface IUnitOfWork
	{
		ICategoryRepository Categories { get; }

		ITransactionRepository Transactions { get; }

		Task CompleteAsync();
	}
}

