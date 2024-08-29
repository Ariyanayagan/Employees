using Employees.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employees.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManger;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManger,
            ILogger<AccountController> logger)
        {
            userManager = _userManager;
            signInManger = _signInManger;
            this.logger = logger;
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

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    //await signInManger.SignInAsync(user, isPersistent: false);

                    TempData["success"] = "Registration successful\n\"Before you can Login, please confirm your " +
                        "email, by clicking on the confirmation link we have emailed you";
                    return View(model);

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

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View();
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["success"] = "Email Verifiation Successfully;";
                ViewBag.ErrorMessage = $"Hey {user.UserName}, Thankyou for Email Verification";
                return View();
            }

            ViewBag.ErrorMessage = "Email cannot be confirmed";
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl=null)
        {
            model.ExternalLogins = (await signInManger.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

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

            // Get the email claim from external login provider (Google, Facebook etc)
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            IdentityUser user = null;

            if (email != null)
            {
                // Find the user
                user = await userManager.FindByEmailAsync(email);

                // If email is not confirmed, display login view with validation error
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
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
                //// Get the email claim value
                //var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    //var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);

                        // After a local user account is created, generate and log the
                        // email confirmation link
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                        new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confirmationLink);

                        TempData["success"] = "Registration successful\n\"Before you can Login, please confirm your " +
                       "email, by clicking on the confirmation link we have emailed you";
                        View("Register");
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await userManager.AddLoginAsync(user, info);
                    await signInManger.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                TempData["error"] = $"Email claim not received from: {info.LoginProvider}";
                TempData["error"] = "Please contact support on aridheena@gmail.com";

                return View("Login", loginViewModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

    }
}
