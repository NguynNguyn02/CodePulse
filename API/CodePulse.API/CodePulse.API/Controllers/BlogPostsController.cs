using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPostDTO;
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

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
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
                     UrlHandle = blogpost.UrlHandle
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
            };
            await _blogPostRepository.CreateAsync(blogpost);
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
            };
            return Ok(reponse);
        }
    }
}
