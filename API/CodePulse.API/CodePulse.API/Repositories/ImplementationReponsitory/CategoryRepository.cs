using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.ImplementationReponsitory
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db=db;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _db.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingCategory is null)
            {
                return null;
            }
            
            _db.Categories.Remove(existingCategory);
            await _db.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _db.Categories.FirstOrDefaultAsync(x=>x.Id==category.Id);
            if (existingCategory != null) { 
                _db.Entry(existingCategory).CurrentValues.SetValues(category);
                await _db.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
