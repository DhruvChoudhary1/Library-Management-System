using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class BorrowController : Controller
    {
        private readonly LibraryContext _context;
    
        public BorrowController(LibraryContext context)
        {
            _context = context;
        }

        // ===========================
        // GET : Borrow/Create
        // ===========================
        public async Task<IActionResult> Create(int? bookId)
        {
            if (bookId == null)
            {
                TempData["Error"] = "Book ID was not provided.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (!book.IsAvailable)
            {
                TempData["Error"] = "This book is currently unavailable.";
                return View("~/Views/Shared/NotAvailable.cshtml");
            }

            BorrowViewModel model = new BorrowViewModel
            {
                BookId = book.Id,
                BookTitle = book.Title
            };

            return View(model);
        }

        // ===========================
        // POST : Borrow/Create
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var book = await _context.Books.FindAsync(model.BookId);

            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (!book.IsAvailable)
            {
                TempData["Error"] = "Book is already borrowed.";
                return View("~/Views/Shared/NotAvailable.cshtml");
            }

            BorrowRecord borrowRecord = new BorrowRecord
            {
                BookId = book.Id,
                BorrowerName = model.BorrowerName!,
                BorrowerEmail = model.BorrowerEmail!,
                Phone = model.Phone!,
                BorrowDate = DateTime.Now
            };

            book.IsAvailable = false;

            _context.BorrowRecords.Add(borrowRecord);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Book borrowed successfully.";

            return RedirectToAction("Index", "Books");
        }

        // ===========================
        // GET : Borrow/Return
        // ===========================
        public async Task<IActionResult> Return(int? borrowRecordId)
        {
            if (borrowRecordId == null)
            {
                TempData["Error"] = "Borrow Record not found.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var borrowRecord = await _context.BorrowRecords
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == borrowRecordId);

            if (borrowRecord == null)
            {
                TempData["Error"] = "Borrow Record not found.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (borrowRecord.ReturnDate != null)
            {
                TempData["Error"] = "Book has already been returned.";
                return View("~/Views/Shared/AlreadyReturned.cshtml");
            }

            ReturnViewModel model = new ReturnViewModel
            {
                BorrowRecordId = borrowRecord.Id,
                BookTitle = borrowRecord.Book?.Title,
                BorrowerName = borrowRecord.BorrowerName,
                BorrowDate = borrowRecord.BorrowDate
            };

            return View(model);
        }

        // ===========================
        // POST : Borrow/Return
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var borrowRecord = await _context.BorrowRecords
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == model.BorrowRecordId);

            if (borrowRecord == null)
            {
                TempData["Error"] = "Borrow Record not found.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (borrowRecord.ReturnDate != null)
            {
                TempData["Error"] = "Book has already been returned.";
                return View("~/Views/Shared/AlreadyReturned.cshtml");
            }

            borrowRecord.ReturnDate = DateTime.Now;

            if (borrowRecord.Book != null)
            {
                borrowRecord.Book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Book returned successfully.";

            return RedirectToAction("Index", "Books");
        }
        // ===========================
// GET : Borrow/History
// ===========================
        public async Task<IActionResult> History()
        {
            var history = await _context.BorrowRecords
            .Include(b => b.Book)
            .OrderByDescending(b => b.BorrowDate)
            .ToListAsync();

            return View(history);
        }
    }
}