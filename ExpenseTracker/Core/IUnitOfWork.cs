using System;
namespace ExpenseTracker.Core
{
	public interface IUnitOfWork
	{
		ICategoryRepository Categories { get; }

		Task CompleteAsync();
	}
}

