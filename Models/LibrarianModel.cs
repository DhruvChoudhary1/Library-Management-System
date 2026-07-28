using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class LibrarianModel
    {
        [Key]
        public int LibrarianId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(18,100)]
        public int Age { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}