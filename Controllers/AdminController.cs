using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // ==========================================
        // CREATE LIBRARIAN GET
        // ==========================================

        [HttpGet]
        public IActionResult CreateLibrarian()
        {
            return View();
        }

        // ==========================================
        // CREATE LIBRARIAN POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLibrarian(
            CreateLibrarianViewModel model)
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
                    "A user with this email already exists.");

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    "Librarian");

            if (!roleResult.Succeeded)
            {
                // Avoid leaving behind a normal user if
                // Librarian role assignment fails.
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            TempData["Success"] =
                "Librarian account created successfully.";

            return RedirectToAction(nameof(CreateLibrarian));
        }
    }
}