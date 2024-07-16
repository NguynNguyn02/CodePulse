using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPostDTO;
using CodePulse.API.Models.DTO.CategoryDTO;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }
        //GET :  https://localhost:7039/api/BlogPosts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPostAsync()
        {
            var blogposts = await _blogPostRepository.GetAllAsync();
            //convert domain to DTO
            var reponse = new List<BlogPostDTO>();
            foreach (var blogpost in blogposts)
            {
                 reponse.Add(new BlogPostDTO
                 {
                     Id = blogpost.Id,
                     Author = blogpost.Author,
                     Content = blogpost.Content,
                     FeaturedImageUrl = blogpost.FeaturedImageUrl,
                     IsVisible = blogpost.IsVisible,
                     PublishedDate = blogpost.PublishedDate,
                     ShortDescription = blogpost.ShortDescription,
                     Title = blogpost.Title,
                     UrlHandle = blogpost.UrlHandle,
                     Categories = blogpost.Categories.Select(x=> new CategoryDTO
                     {
                         Id=x.Id,
                         Name = x.Name,
                         UrlHandle=x.UrlHandle,
                     }).ToList()
                 });

            }
            return Ok(reponse);

        }
        //POST : https://localhost:7039/api/BlogPosts
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequestDTO request)
        {
            //convert DTO to domain model
            var blogpost = new BlogPost
            {
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };
            foreach (var categoryGuid in request.Categories) {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }
            blogpost = await _blogPostRepository.CreateAsync(blogpost);
            //convert domain model to DTO
            var reponse = new BlogPostDTO
            {
                Id = blogpost.Id,
                Title = blogpost.Title,
                Author = blogpost.Author,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                PublishedDate = blogpost.PublishedDate,
                ShortDescription = blogpost.ShortDescription,
                UrlHandle = blogpost.UrlHandle,
                IsVisible = blogpost.IsVisible,
                Categories = blogpost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(reponse);
        }

        //GET : {apiBaseUrl}/api/BlogPosts/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogpost = await _blogPostRepository.GetByIdAsync(id);
            if (blogpost is null)
            {
                return NotFound();
            }
            // convert domain model to DTO
            var reponse = new BlogPostDTO()
            {
                Id = blogpost.Id,
                Title = blogpost.Title,
                Author = blogpost.Author,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                PublishedDate = blogpost.PublishedDate,
                ShortDescription = blogpost.ShortDescription,
                UrlHandle = blogpost.UrlHandle,
                IsVisible = blogpost.IsVisible,
                Categories = blogpost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(reponse);
        }
    }
}
