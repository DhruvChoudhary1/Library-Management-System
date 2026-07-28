using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.ViewModels
{
    public class BorrowViewModel
    {
        [Required]
        public int BookId { get; set; }

        [BindNever]
        public string? BookTitle { get; set; }

        [Required(ErrorMessage = "Borrower name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? BorrowerName { get; set; }

        [Required(ErrorMessage = "Borrower email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? BorrowerEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }
    }
}