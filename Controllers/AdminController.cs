using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Saudi_FormEmail.Models;
using Saudi_FormEmail.Resources;

namespace Saudi_FormEmail.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IStringLocalizer<SharedResource> localizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    if (result.Succeeded && await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("ViewSubmissions", "Form");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt or not an Admin.");
            }

            return View(model);
        }
    }
}
