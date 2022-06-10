using AzTUChat.Model;
using AzTUChat.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzTUChat.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM signInVm, string ReturnUrl)
        {
            AppUser appUser;
            if (signInVm.UserNameOrEmail.Contains("@"))
            {
                appUser = await _userManager.FindByIdAsync(signInVm.UserNameOrEmail);
            }
            else
            {
                appUser = await _userManager.FindByNameAsync(signInVm.UserNameOrEmail);
            }
            if (appUser == null)
            {
                ModelState.AddModelError("", "Wrong usernme or pass!");
                return View(signInVm);
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, signInVm.Password, signInVm.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try later");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong usernme or pass!");
                return View(signInVm);
            }
            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Register));
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser nUser = new AppUser()
            {

                Name = registerVM.FirstName,
                Surname = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Username
            };
            IdentityResult result = await _userManager.CreateAsync(nUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
