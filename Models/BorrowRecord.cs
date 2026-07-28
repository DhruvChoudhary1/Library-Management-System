using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Borrower name is required.")]
        [StringLength(100)]
        public string BorrowerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string BorrowerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Foreign Key
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }
    }
}