using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Models
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<LibrarianModel> Librarians { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Newspaper> Newspapers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            modelBuilder.Entity<Book>().HasData(

                new Book
                {
                    Id = 1,
                    Title = "The Alchemist",
                    Author = "Paulo Coelho",
                    Genre = "Fiction",
                    PublicationYear = 1988,
                    IsAvailable = true
                },

                new Book
                {
                    Id = 2,
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    Genre = "Self Help",
                    PublicationYear = 2018,
                    IsAvailable = true
                },

                new Book
                {
                    Id = 3,
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Genre = "Programming",
                    PublicationYear = 2008,
                    IsAvailable = true
                },

                new Book
                {
                    Id = 4,
                    Title = "Introduction to Algorithms",
                    Author = "Thomas H. Cormen",
                    Genre = "Computer Science",
                    PublicationYear = 2009,
                    IsAvailable = true
                },

                new Book
                {
                    Id = 5,
                    Title = "Rich Dad Poor Dad",
                    Author = "Robert Kiyosaki",
                    Genre = "Finance",
                    PublicationYear = 1997,
                    IsAvailable = true
                }

            );

            modelBuilder.Entity<StudentModel>().HasData(

                new StudentModel
                {
                    StudentId = 1,
                    StudentName = "Alice Johnson",
                    Email = "alice@email.com",
                    Phone = "555-0101"
                },

                new StudentModel
                {
                    StudentId = 2,
                    StudentName = "Bob Smith",
                    Email = "bob@email.com",
                    Phone = "555-0102"
                },

                new StudentModel
                {
                    StudentId = 3,
                    StudentName = "Charlie Brown",
                    Email = "charlie@email.com",
                    Phone = "555-0103"
                }

            );
            modelBuilder.Entity<LibrarianModel>().HasData(

    new LibrarianModel
    {
        LibrarianId = 1,
        Name = "Sarah Connor",
        Age = 34,
        Phone = "555-0201"
    },

    new LibrarianModel
    {
        LibrarianId = 2,
        Name = "John Doe",
        Age = 28,
        Phone = "555-0202"
    },

    new LibrarianModel
    {
        LibrarianId = 3,
        Name = "Michael Scott",
        Age = 45,
        Phone = "555-0203"
    }

);
modelBuilder.Entity<Magazine>().HasData(

    new Magazine
    {
        Id = 1,
        Title = "National Geographic",
        Publisher = "National Geographic Society",
        Category = "Science",
        IssueDate = new DateTime(2026, 1, 1),
        IsAvailable = true
    },

    new Magazine
    {
        Id = 2,
        Title = "TIME",
        Publisher = "Time USA LLC",
        Category = "News",
        IssueDate = new DateTime(2026, 2, 1),
        IsAvailable = true
    },

    new Magazine
    {
        Id = 3,
        Title = "Forbes",
        Publisher = "Forbes Media",
        Category = "Business",
        IssueDate = new DateTime(2026, 3, 1),
        IsAvailable = true
    }

);
modelBuilder.Entity<Newspaper>().HasData(

    new Newspaper
    {
        Id = 1,
        Title = "The Times of India",
        Publisher = "Bennett Coleman",
        PublicationDate = new DateTime(2026, 1, 1),
        IsAvailable = true
    },

    new Newspaper
    {
        Id = 2,
        Title = "The Hindu",
        Publisher = "The Hindu Group",
        PublicationDate = new DateTime(2026, 1, 2),
        IsAvailable = true
    },

    new Newspaper
    {
        Id = 3,
        Title = "Indian Express",
        Publisher = "Express Group",
        PublicationDate = new DateTime(2026, 1, 3),
        IsAvailable = true
    }

);
        }
    }
}