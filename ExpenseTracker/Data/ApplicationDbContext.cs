using System;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data
{
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}

