using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.ImplementationReponsitory;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDTO request)
        {
            //MAP DTO to domain model

            var category = new Category()
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };
            await _categoryRepository.CreateAsync(category);

            // Domain model to DTO

            var response = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);
        }
        //GET   'https://localhost:7039/api/Categories'
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            //map domain model to DTO

            var response = new List<CategoryDTO>();
            foreach (var item in categories)
            {
                response.Add(new CategoryDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    UrlHandle = item.UrlHandle
                });
            }
            return Ok(response);
        }

        //GET https://localhost:7039/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoriesByID([FromRoute] Guid id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDTO()
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }
        //PUT https://localhost:7039/api/Categories/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDTO request)
        {
            //convert DTO to domain model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category =  await _categoryRepository.UpdateAsync(category);

            //convert domain model to DTO

            if (category is null) { 
                return NotFound();
            }
            var response = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

    }
}
