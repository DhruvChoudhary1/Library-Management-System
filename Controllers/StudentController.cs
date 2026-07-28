using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class StudentController : Controller
    {
        private readonly LibraryContext _context;

        public StudentController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
{
    const int pageSize = 5;

    var query = _context.Students.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        searchTerm = searchTerm.Trim();

        query = query.Where(s =>
            s.StudentName.Contains(searchTerm) ||
            s.Email.Contains(searchTerm) ||
            s.Phone.Contains(searchTerm));
    }

    int totalRecords = await query.CountAsync();

    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    if (page < 1)
        page = 1;

    if (totalPages > 0 && page > totalPages)
        page = totalPages;

    var students = await query
        .OrderBy(s => s.StudentName)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    StudentIndexViewModel vm = new()
    {
        Students = students,
        SearchTerm = searchTerm,
        CurrentPage = page,
        TotalPages = totalPages,
        PageSize = pageSize
    };

    return View(vm);
}

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Students.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Student added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Student/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Student updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Student deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}