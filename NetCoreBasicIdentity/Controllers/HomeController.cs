using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreBasicIdentity.Entities;
using NetCoreBasicIdentity.Models;

namespace NetCoreBasicIdentity.Controllers
{
    [AutoValidateAntiforgeryToken]
    
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel userCreateModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = userCreateModel.UserName,
                    Email = userCreateModel.Email,
                    Gender = userCreateModel.Gender
                };
                var identityResult = await _userManager.CreateAsync(appUser, userCreateModel.Password);
                if (identityResult.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                            CreatedDate = DateTime.Now
                        });
                    }


                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(userCreateModel);
        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            return View(new UserSignInModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);
                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("Panel");
                    }
                }

                else if (signInResult.IsLockedOut)
                {
                    var lockEnd = await _userManager.GetLockoutEndDateAsync(user);
                    ModelState.AddModelError("", "Hesabınız askıya alındı. Lütfen daha sonra tekrar deneyiniz.");
                }
                else
                {
                    var message = string.Empty;
                    if (user != null)
                    {
                        var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                        message =
                            $"{(_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount)} kez daha girerseniz hesabınız geçiçi olarak kilitlenecektir.";
                    }
                    else
                    {
                        message = "Kullanıcı adı veya şifre hatalı";
                    }

                    ModelState.AddModelError("", message);
                }
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            User.IsInRole("Member");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult Panel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult PrivatePage()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}