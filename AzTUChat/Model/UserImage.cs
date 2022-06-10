using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzTUChat.Model
{
    public class UserImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
