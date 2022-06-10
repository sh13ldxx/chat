using Microsoft.AspNetCore.Identity;

namespace AzTUChat.Model
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ConnectionId { get; set; }
        public UserImage Userİmage { get; set; }
    }
}
