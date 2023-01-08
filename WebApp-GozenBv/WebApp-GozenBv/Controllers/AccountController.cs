using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebApp_GozenBv.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApp_GozenBv.Controllers
{
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region register
        [Authorize]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (await UserExistAsync(user))
                {
                    ModelState.AddModelError("", "Email adres is al geregistreerd");
                    return View(user);
                }

                if (!user.Password.Equals(user.ConfirmPassword))
                {
                    ModelState.AddModelError("", "wachtwoord moet identiek zijn!");

                }
                else
                {
                    var result = await RegisterUserAsync(user);
                    if (result.Succeeded)
                    {
                        return View("RegistrationCompleted", result.CreatedUser);
                    }
                    else
                    {
                        foreach (string error in result.Errors)

                            ModelState.AddModelError("", error);
                        return View(user);

                    }
                    //var identityUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                    //await _userManager.CreateAsync(identityUser, user.Password);
                    ////registration process
                    //var createdUser = new CreatedUser();
                    //createdUser.IdentityUser = identityUser;
                    ////fill properties

                }

            }
            return View(user);

        }

        // Errors worden weergeven door dit!! (password) 
        private async Task<RegisterResult> RegisterUserAsync(RegisterViewModel user)
        {
            var registerResult = new RegisterResult();
            var identityUser = new IdentityUser { UserName = user.Email, Email = user.Email };
            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                var createdUser = new CreatedUser { CreationDate = DateTime.Now, IdentityUser = identityUser };
                registerResult.Succeeded = true;
                registerResult.CreatedUser = createdUser;
            }
            else
            {
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        registerResult.Errors.Add(error.Description);
                    }
                }
                else
                {
                    registerResult.Errors.Add("Er is een probleem om de gebruiker te registreren!");
                }

            }
            return registerResult;
        }

        // zoeken of email al bestaat!!
        private async Task<bool> UserExistAsync(RegisterViewModel user)
        {
            bool userExist = false;
            var result = await _userManager.FindByEmailAsync(user.Email);
            if (result != null)
            {
                userExist = true;
            }
            return userExist;
        }
        #endregion

        #region login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var identityUser = await _userManager.FindByEmailAsync(user.Email);
                    if (identityUser != null)
                        return View("LoginCompleted", identityUser);
                    else
                        ModelState.AddModelError("", "Invalid login attempt.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

            }
            return View(user);
        }
        #endregion

        #region logout 
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return View("LogoutCompleted");
        }
        #endregion
    }
}
