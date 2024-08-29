using Employees.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Security.Claims;

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

        [HttpGet("login")]

        public async Task<IActionResult> login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel()
            {
                returnUrl = returnUrl,
                ExternalLogins =(await signInManger.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
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

                    if (Request.Headers["Accept"] == "application/json")
                    {
                        // Return JWT token as JSON for API clients
                        return Ok(new { Token = "token" });
                    }


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

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnurl)
        {
            var redirect = Url.Action("ExternalLoginCallback","Account", new { ReturnUrl = returnurl });
            var properties = signInManger.ConfigureExternalAuthenticationProperties(provider, redirect);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                returnUrl = returnUrl,
                ExternalLogins =
                        (await signInManger.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await signInManger.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await signInManger.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await userManager.AddLoginAsync(user, info);
                    await signInManger.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }


    }
}
