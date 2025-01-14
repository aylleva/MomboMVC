using MambaMVC.Models;
using MambaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MambaMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> usermeneger;
        private readonly SignInManager<AppUser> signinuser;
        private readonly RoleManager<IdentityRole> userrole;

        public AccountController(UserManager<AppUser> usermeneger, SignInManager<AppUser> signinuser, RoleManager<IdentityRole> userrole)
        {
            this.usermeneger = usermeneger;
            this.signinuser = signinuser;
            this.userrole = userrole;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM uservm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new()
            {
                Name = uservm.Name,
                Email = uservm.Email,
                Surname = uservm.Surname,
                UserName = uservm.UserName,
            };

            var result = await usermeneger.CreateAsync(user, uservm.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return View();  
            }

            
        }
    }
}
