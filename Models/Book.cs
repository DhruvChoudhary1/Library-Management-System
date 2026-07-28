using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1000, 3000)]
        public int PublicationYear { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation Property
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}