using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class DashboardController : Controller
    {
        private readonly LibraryContext _context;

        public DashboardController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            DashboardModel model = new DashboardModel
            {
                TotalBooks = await _context.Books.CountAsync(),

                AvailableBooks = await _context.Books
                    .CountAsync(b => b.IsAvailable),

                BorrowedBooks = await _context.Books
                    .CountAsync(b => !b.IsAvailable),

                // Student & Librarian modules will be added later
                TotalStudents = await _context.Students.CountAsync(),

                TotalLibrarians = await _context.Librarians.CountAsync()
            };

            return View(model);
        }
    }
}