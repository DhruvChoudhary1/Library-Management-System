using LibraryManagement.Controllers;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManagement.Tests
{
    public class BooksControllerTests
    {
        // =========================================================
        // Creates a separate InMemory database for every test
        // =========================================================
        private LibraryContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new LibraryContext(options);
        }

        // =========================================================
        // Creates controller with TempData configured
        // =========================================================
        private BooksController CreateController(LibraryContext context)
        {
            var controller = new BooksController(context);

            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                new TestTempDataProvider()
            );

            return controller;
        }

        // =========================================================
        // BASIC CONTEXT TEST
        // =========================================================

        [Fact]
        public void CreateContext_ReturnsValidContext()
        {
            using var context = CreateContext();

            Assert.NotNull(context);
            Assert.NotNull(context.Books);
        }

        // =========================================================
        // INDEX TESTS
        // =========================================================

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result = await controller.Index(null, 1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsBookListViewModel()
        {
            using var context = CreateContext();

            context.Books.Add(new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                Genre = "Testing",
                PublicationYear = 2026,
                IsAvailable = true
            });

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.Index(null, 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(viewResult.Model);

            Assert.Single(model.Books);
        }

        [Fact]
        public async Task Index_SearchByTitle_ReturnsMatchingBook()
        {
            using var context = CreateContext();

            context.Books.AddRange(
                new Book
                {
                    Title = "Clean Code",
                    Author = "Robert Martin",
                    Genre = "Programming",
                    PublicationYear = 2008,
                    IsAvailable = true
                },
                new Book
                {
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    Genre = "Self Help",
                    PublicationYear = 2018,
                    IsAvailable = true
                }
            );

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index("Clean", 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Single(model.Books);

            Assert.Equal(
                "Clean Code",
                model.Books.First().Title);
        }

        [Fact]
        public async Task Index_SearchByAuthor_ReturnsMatchingBook()
        {
            using var context = CreateContext();

            context.Books.AddRange(
                new Book
                {
                    Title = "Book One",
                    Author = "James Clear",
                    Genre = "Self Help",
                    PublicationYear = 2018,
                    IsAvailable = true
                },
                new Book
                {
                    Title = "Book Two",
                    Author = "Robert Martin",
                    Genre = "Programming",
                    PublicationYear = 2008,
                    IsAvailable = true
                }
            );

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index("James", 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Single(model.Books);

            Assert.Equal(
                "James Clear",
                model.Books.First().Author);
        }

        [Fact]
        public async Task Index_SearchByGenre_ReturnsMatchingBook()
        {
            using var context = CreateContext();

            context.Books.AddRange(
                new Book
                {
                    Title = "Book One",
                    Author = "Author One",
                    Genre = "Finance",
                    PublicationYear = 2020,
                    IsAvailable = true
                },
                new Book
                {
                    Title = "Book Two",
                    Author = "Author Two",
                    Genre = "Programming",
                    PublicationYear = 2021,
                    IsAvailable = true
                }
            );

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index("Finance", 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Single(model.Books);

            Assert.Equal(
                "Finance",
                model.Books.First().Genre);
        }

        [Fact]
        public async Task Index_SearchWithNoMatch_ReturnsEmptyList()
        {
            using var context = CreateContext();

            context.Books.Add(new Book
            {
                Title = "Clean Code",
                Author = "Robert Martin",
                Genre = "Programming",
                PublicationYear = 2008,
                IsAvailable = true
            });

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index("NonexistentBook", 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Empty(model.Books);
        }

        [Fact]
        public async Task Index_Pagination_ReturnsFiveBooksPerPage()
        {
            using var context = CreateContext();

            for (int i = 1; i <= 7; i++)
            {
                context.Books.Add(new Book
                {
                    Title = $"Book {i}",
                    Author = $"Author {i}",
                    Genre = "Testing",
                    PublicationYear = 2020 + i,
                    IsAvailable = true
                });
            }

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index(null, 1);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Equal(5, model.Books.Count());
            Assert.Equal(1, model.CurrentPage);
            Assert.Equal(2, model.TotalPages);
            Assert.Equal(5, model.PageSize);
        }

        [Fact]
        public async Task Index_SecondPage_ReturnsRemainingBooks()
        {
            using var context = CreateContext();

            for (int i = 1; i <= 7; i++)
            {
                context.Books.Add(new Book
                {
                    Title = $"Book {i}",
                    Author = $"Author {i}",
                    Genre = "Testing",
                    PublicationYear = 2020,
                    IsAvailable = true
                });
            }

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Index(null, 2);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<BookListViewModel>(
                    viewResult.Model);

            Assert.Equal(2, model.Books.Count());
            Assert.Equal(2, model.CurrentPage);
            Assert.Equal(2, model.TotalPages);
        }

        // =========================================================
        // DETAILS TESTS
        // =========================================================

        [Fact]
        public async Task Details_ExistingBook_ReturnsBook()
        {
            using var context = CreateContext();

            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                Genre = "Testing",
                PublicationYear = 2026,
                IsAvailable = true
            };

            context.Books.Add(book);

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Details(book.Id);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<Book>(viewResult.Model);

            Assert.Equal(
                "Test Book",
                model.Title);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result =
                await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // =========================================================
        // CREATE TESTS
        // =========================================================

        [Fact]
        public void Create_Get_ReturnsView()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_ValidBook_AddsBookToDatabase()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var book = new Book
            {
                Title = "New Book",
                Author = "New Author",
                Genre = "Technology",
                PublicationYear = 2026
            };

            var result =
                await controller.Create(book);

            Assert.Equal(
                1,
                await context.Books.CountAsync());

            var savedBook =
                await context.Books.FirstAsync();

            Assert.Equal(
                "New Book",
                savedBook.Title);

            Assert.True(savedBook.IsAvailable);

            Assert.Equal(
                "Book added successfully.",
                controller.TempData["Success"]);

            var redirect =
                Assert.IsType<RedirectToActionResult>(
                    result);

            Assert.Equal(
                nameof(BooksController.Index),
                redirect.ActionName);
        }

        [Fact]
        public async Task Create_InvalidModel_DoesNotAddBook()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            controller.ModelState.AddModelError(
                "Title",
                "Title is required.");

            var book = new Book
            {
                Title = "",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2026
            };

            var result =
                await controller.Create(book);

            Assert.Empty(context.Books);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            Assert.Same(
                book,
                viewResult.Model);
        }

        // =========================================================
        // EDIT TESTS
        // =========================================================

        [Fact]
        public async Task Edit_GetExistingBook_ReturnsBook()
        {
            using var context = CreateContext();

            var book = new Book
            {
                Title = "Old Title",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2020,
                IsAvailable = true
            };

            context.Books.Add(book);

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Edit(book.Id);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<Book>(
                    viewResult.Model);

            Assert.Equal(
                "Old Title",
                model.Title);
        }

        [Fact]
        public async Task Edit_GetInvalidBook_ReturnsNotFound()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result =
                await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_IdDoesNotMatchBookId_ReturnsNotFound()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var book = new Book
            {
                Id = 1,
                Title = "Test",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2026,
                IsAvailable = true
            };

            var result =
                await controller.Edit(2, book);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ValidBook_UpdatesDatabase()
        {
            using var context = CreateContext();

            var book = new Book
            {
                Title = "Old Title",
                Author = "Old Author",
                Genre = "Testing",
                PublicationYear = 2020,
                IsAvailable = true
            };

            context.Books.Add(book);

            await context.SaveChangesAsync();

            int bookId = book.Id;

            // Simulates receiving a fresh object
            // from an HTTP form.
            context.ChangeTracker.Clear();

            var controller = CreateController(context);

            var updatedBook = new Book
            {
                Id = bookId,
                Title = "Updated Title",
                Author = "Updated Author",
                Genre = "Programming",
                PublicationYear = 2026,
                IsAvailable = true
            };

            var result =
                await controller.Edit(
                    bookId,
                    updatedBook);

            var savedBook =
                await context.Books
                    .AsNoTracking()
                    .FirstAsync(b => b.Id == bookId);

            Assert.Equal(
                "Updated Title",
                savedBook.Title);

            Assert.Equal(
                "Updated Author",
                savedBook.Author);

            Assert.Equal(
                "Programming",
                savedBook.Genre);

            Assert.Equal(
                2026,
                savedBook.PublicationYear);

            Assert.Equal(
                "Book updated successfully.",
                controller.TempData["Success"]);

            var redirect =
                Assert.IsType<RedirectToActionResult>(
                    result);

            Assert.Equal(
                nameof(BooksController.Index),
                redirect.ActionName);
        }

        [Fact]
        public async Task Edit_InvalidModel_ReturnsView()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            controller.ModelState.AddModelError(
                "Title",
                "Title is required.");

            var book = new Book
            {
                Id = 1,
                Title = "",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2026
            };

            var result =
                await controller.Edit(1, book);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            Assert.Same(
                book,
                viewResult.Model);
        }

        // =========================================================
        // DELETE TESTS
        // =========================================================

        [Fact]
        public async Task Delete_GetExistingBook_ReturnsBook()
        {
            using var context = CreateContext();

            var book = new Book
            {
                Title = "Delete Test",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2026,
                IsAvailable = true
            };

            context.Books.Add(book);

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.Delete(book.Id);

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsType<Book>(
                    viewResult.Model);

            Assert.Equal(
                book.Id,
                model.Id);
        }

        [Fact]
        public async Task Delete_GetInvalidBook_ReturnsNotFound()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result =
                await controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ExistingBook_RemovesBook()
        {
            using var context = CreateContext();

            var book = new Book
            {
                Title = "Delete Me",
                Author = "Author",
                Genre = "Testing",
                PublicationYear = 2026,
                IsAvailable = true
            };

            context.Books.Add(book);

            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result =
                await controller.DeleteConfirmed(
                    book.Id);

            Assert.Empty(context.Books);

            Assert.Equal(
                "Book deleted successfully.",
                controller.TempData["Success"]);

            var redirect =
                Assert.IsType<RedirectToActionResult>(
                    result);

            Assert.Equal(
                nameof(BooksController.Index),
                redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_InvalidId_StillRedirectsToIndex()
        {
            using var context = CreateContext();

            var controller = CreateController(context);

            var result =
                await controller.DeleteConfirmed(999);

            Assert.Empty(context.Books);

            var redirect =
                Assert.IsType<RedirectToActionResult>(
                    result);

            Assert.Equal(
                nameof(BooksController.Index),
                redirect.ActionName);
        }
    }

    // =============================================================
    // Test-only TempData Provider
    // =============================================================

    public class TestTempDataProvider : ITempDataProvider
    {
        private Dictionary<string, object> _data = new();

        public IDictionary<string, object> LoadTempData(
            HttpContext context)
        {
            return _data;
        }

        public void SaveTempData(
            HttpContext context,
            IDictionary<string, object> values)
        {
            _data =
                new Dictionary<string, object>(values);
        }
    }
}