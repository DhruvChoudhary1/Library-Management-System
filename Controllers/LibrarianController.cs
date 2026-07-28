using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class LibrarianController : Controller
    {
        private readonly LibraryContext _context;

        public LibrarianController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Librarian
        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
{
    const int pageSize = 5;

    var query = _context.Librarians.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        searchTerm = searchTerm.Trim();

        query = query.Where(l =>
            l.Name.Contains(searchTerm) ||
            l.Phone.Contains(searchTerm));
    }

    int totalRecords = await query.CountAsync();

    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    if (page < 1)
        page = 1;

    if (totalPages > 0 && page > totalPages)
        page = totalPages;

    var librarians = await query
        .OrderBy(l => l.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    LibrarianIndexViewModel vm = new()
    {
        Librarians = librarians,
        SearchTerm = searchTerm,
        CurrentPage = page,
        TotalPages = totalPages,
        PageSize = pageSize
    };

    return View(vm);
}

        // GET: Librarian/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Librarian/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibrarianModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Librarians.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Librarian added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Librarian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var librarian = await _context.Librarians.FindAsync(id);

            if (librarian == null)
                return NotFound();

            return View(librarian);
        }

        // POST: Librarian/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibrarianModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Librarian updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Librarian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var librarian = await _context.Librarians.FindAsync(id);

            if (librarian == null)
                return NotFound();

            _context.Librarians.Remove(librarian);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Librarian deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}