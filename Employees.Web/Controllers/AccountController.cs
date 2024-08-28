using Employees.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManger;

        public AccountController(UserManager<IdentityUser> _userManager,SignInManager<IdentityUser> _signInManger)
        {
            userManager = _userManager;
            signInManger = _signInManger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user,model.Password);

                if (result.Succeeded) { 
                    
                    await signInManger.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "User Login Successfully!";
                    return RedirectToAction("index", "employee");
                
                }

                if (result.Errors.Count() == 1)
                {
                    TempData["error"] = result.Errors.First().Description;
                }
                else if (result.Errors.Count() > 1)
                {
                    List<string> errors = new List<string>();

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        errors.Add(error.Description);
                    }

                    TempData["errorList"] = errors;
                }
            
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl=null)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManger.PasswordSignInAsync(model.Email, model.Password, model.rememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        TempData["success"] = "User logged in successfully!";
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        TempData["success"] = "User logged in successfully!";
                        return RedirectToAction("Index", "Employee");
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
                TempData["error"] = "Invalid username or password.";
            }
            TempData["error"] = "Error occur in login!";
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await signInManger.SignOutAsync();
            return RedirectToAction("index","employee");
        }


    }
}
