
using AzTUChat.Model;
using System.Collections.Generic;

namespace AzTUChat.ViewModel
{
    public class HomeVm
    {
        public IEnumerable<AppUser> Users { get; set; }
        public AppUser CurrentUser { get; set; }
        public string ConnectionId { get; set; }

    }
}
