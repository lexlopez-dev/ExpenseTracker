using System;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public override async Task<Category?> GetById(int id)
        {
            try
            {
                return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesByType(string type)
        {
            try
            {
                return await _context.Categories.Where(x => x.Type == type).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(x => x.CategoryId == id);
        }
    }
}

