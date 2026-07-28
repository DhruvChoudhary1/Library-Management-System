using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class MagazineController : Controller
    {
        private readonly LibraryContext _context;

        public MagazineController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchQuery, int page = 1)
        {
            const int pageSize = 5;

            var query = _context.Magazines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(m =>
                    m.Title.Contains(searchQuery) ||
                    m.Publisher.Contains(searchQuery) ||
                    m.Category.Contains(searchQuery));
            }

            int totalRecords = await query.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var magazines = await query
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new MagazineListViewModel
            {
                Magazines = magazines,
                SearchQuery = searchQuery,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(vm);
        }
        [Authorize(Roles = "Admin,Librarian")]

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Create(Magazine model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Magazines.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Librarian")]

        public async Task<IActionResult> Edit(int id)
        {
            var magazine = await _context.Magazines.FindAsync(id);

            if (magazine == null)
                return NotFound();

            return View(magazine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(Magazine model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Delete(int id)
        {
            var magazine = await _context.Magazines.FindAsync(id);

            if (magazine == null)
                return NotFound();

            _context.Magazines.Remove(magazine);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}