using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.InterfaceRepository;

namespace CodePulse.API.Repositories.ImplementationReponsitory
{
    
    public class BlogImageRepository : IBlogImageRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogImageRepository(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1. Upload the Image to API/Image Folder 
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath,"Images",$"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2. Update the database
            // ex: https://codepulse.com/images/somefilename.jpg 
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await _db.BlogImages.AddAsync(blogImage);
            await _db.SaveChangesAsync();

            return blogImage;
        }
    }
}
