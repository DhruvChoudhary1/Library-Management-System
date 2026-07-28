using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class NewspaperController : Controller
    {
        private readonly LibraryContext _context;

        public NewspaperController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchQuery, int page = 1)
        {
            const int pageSize = 5;

            var query = _context.Newspapers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(n =>
                    n.Title.Contains(searchQuery) ||
                    n.Publisher.Contains(searchQuery));
            }

            int totalRecords = await query.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var newspapers = await query
                .OrderBy(n => n.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(new NewspaperListViewModel
            {
                Newspapers = newspapers,
                SearchQuery = searchQuery,
                CurrentPage = page,
                TotalPages = totalPages
            });
        }

        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Create(Newspaper model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Newspapers.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(int id)
        {
            var newspaper = await _context.Newspapers.FindAsync(id);

            if (newspaper == null)
                return NotFound();

            return View(newspaper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(Newspaper model)
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
            var newspaper = await _context.Newspapers.FindAsync(id);

            if (newspaper == null)
                return NotFound();

            _context.Newspapers.Remove(newspaper);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}