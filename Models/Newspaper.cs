using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Newspaper
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        public DateTime PublicationDate { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}