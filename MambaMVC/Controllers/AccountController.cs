using MambaMVC.Models;
using MambaMVC.Utilities.Enums;
using MambaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

            await usermeneger.AddToRoleAsync(user,UserRoles.Member.ToString());
             await signinuser.SignInAsync(user,false);
            return RedirectToAction(nameof(HomeController.Index),"Home");
            
        }

        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm,string? returnurl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user=await usermeneger.Users.FirstOrDefaultAsync(u=>u.UserName==loginvm.UserNameorEmail ||u.Email==loginvm.UserNameorEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }

            var result=await signinuser.PasswordSignInAsync(user,loginvm.Password,false,true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your Account has been blocked");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }

            if(returnurl is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");  
            }
            return Redirect(returnurl);


            
        }

     
        public async Task<IActionResult> Logout()
        {
            await signinuser.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }




        public async Task<IActionResult> CreateRoles()
        {
            foreach(var roles in Enum.GetValues(typeof(UserRoles)))
            {
                if(!await userrole.RoleExistsAsync(roles.ToString()))
                {
                    await userrole.CreateAsync(new IdentityRole { Name = roles.ToString() });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
