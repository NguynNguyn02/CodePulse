using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogImageDTO;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IBlogImageRepository _blogImage;
        public ImagesController(IBlogImageRepository blogImage)
        {
            _blogImage = blogImage;
        }

        //GET : {apiurlbase}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _blogImage.GetAllAsync();

            //convert domain model to DTO

            var reponse = new List<BlogImageDTO>();
            foreach (var image in images)
            {
                reponse.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Title = image.Title,
                    Url = image.Url
                });
            }
            return Ok(reponse);
        }



        //POST : {apiurlbase}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                //file upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,

                };
                blogImage = await _blogImage.Upload(file, blogImage);
                //convert domain model to DTO

                var reponse = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    FileExtension = blogImage.FileExtension,
                    Title = title,
                    DateCreated = blogImage.DateCreated,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };

                return Ok(reponse);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Định dạng file không hỗ trợ");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "Kích thước file không thể lớn hơn 10MB");
            }
        }
    }

}
