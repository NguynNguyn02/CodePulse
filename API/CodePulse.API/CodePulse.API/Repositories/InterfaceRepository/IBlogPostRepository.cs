using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.InterfaceRepository
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> CreateAsync(BlogPost blogPost);
    }
}
