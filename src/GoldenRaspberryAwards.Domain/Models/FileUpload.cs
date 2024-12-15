using Microsoft.AspNetCore.Http;

namespace GoldenRaspberryAwards.Domain.Models
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }
}
