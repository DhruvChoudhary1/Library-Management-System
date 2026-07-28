using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}