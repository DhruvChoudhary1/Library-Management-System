using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // Display all books
        public async Task<IActionResult> Index(string? searchQuery, int page = 1)
{
    const int pageSize = 5;

    var query = _context.Books
        .Include(b => b.BorrowRecords)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchQuery))
    {
        searchQuery = searchQuery.Trim();

        query = query.Where(b =>
            b.Title.Contains(searchQuery) ||
            b.Author.Contains(searchQuery) ||
            b.Genre.Contains(searchQuery));
    }

    int totalRecords = await query.CountAsync();

    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    if (page < 1)
        page = 1;

    if (totalPages > 0 && page > totalPages)
        page = totalPages;

    var books = await query
        .OrderBy(b => b.Title)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    BookListViewModel vm = new()
    {
        Books = books,
        SearchQuery = searchQuery,
        CurrentPage = page,
        TotalPages = totalPages,
        PageSize = pageSize
    };

    return View(vm);
}

        // Book Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // Create Book (GET)
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        // Create Book (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book.IsAvailable = true;

                _context.Add(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book added successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // Edit Book (GET)
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // Edit Book (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Book updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // Delete Book (GET)
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // Delete Book (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}