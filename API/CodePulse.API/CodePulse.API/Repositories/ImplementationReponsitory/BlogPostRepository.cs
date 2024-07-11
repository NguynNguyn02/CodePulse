using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.ImplementationReponsitory
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogPostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _db.BlogPosts.AddAsync(blogPost);
            await _db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _db.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        
    }
}
