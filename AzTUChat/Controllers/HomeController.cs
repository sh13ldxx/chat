using AzTUChat.DAL;
using AzTUChat.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzTUChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public AppDbContext _context { get; }

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>  Index()
        {

           
            var guid = Guid.NewGuid().ToString();
            HomeVm homeVm = new HomeVm
            {
                Users = _context.Users.Where(x=>x.UserName !=User.Identity.Name).Include(x=>x.Userİmage),
                CurrentUser=_context.Users.SingleOrDefault(x=>x.UserName==User.Identity.Name),                
            };

            
            return View(homeVm);
        }
    }
}
