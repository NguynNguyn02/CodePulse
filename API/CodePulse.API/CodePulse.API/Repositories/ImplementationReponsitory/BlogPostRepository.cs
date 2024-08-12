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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var extistingBlogPost = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (extistingBlogPost == null) { 
                return null;
            }
            _db.BlogPosts.Remove(extistingBlogPost);
            await _db.SaveChangesAsync();
            return extistingBlogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _db.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _db.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _db.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var extistingBlogPost = await _db.BlogPosts
                .Include(x=>x.Categories).FirstOrDefaultAsync(x=> x.Id == blogPost.Id);
            
            if (extistingBlogPost == null) { 
                return null;
            }

            //Update BlogPost
            _db.Entry(extistingBlogPost).CurrentValues.SetValues(blogPost);
            //Update Category
            extistingBlogPost.Categories = blogPost.Categories;
                
            await _db.SaveChangesAsync();
            return blogPost;
        }
    }
}
