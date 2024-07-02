using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.InterfaceRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
    }
}
