using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==========================================
        // REGISTER GET
        // ==========================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // ==========================================
        // REGISTER POST
        // ==========================================

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists.");

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(
                    user,
                    isPersistent: false);

                TempData["Success"] =
                    "Account created successfully.";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return View(model);
        }

        // ==========================================
        // LOGIN GET
        // ==========================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        // ==========================================
        // LOGIN POST
        // ==========================================

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["Success"] =
                    "Login successful.";

                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
                    Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password.");

            return View(model);
        }

        // ==========================================
        // LOGOUT POST
        // ==========================================

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["Success"] =
                "You have been logged out.";

            return RedirectToAction(
                nameof(Login));
        }

        // ==========================================
        // ACCESS DENIED
        // ==========================================

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}