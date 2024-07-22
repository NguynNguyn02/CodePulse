using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.InterfaceRepository
{
    public interface IBlogImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
