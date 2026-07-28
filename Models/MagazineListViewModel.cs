using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class MagazineListViewModel
    {
        public IEnumerable<Magazine> Magazines { get; set; } = new List<Magazine>();

        public string? SearchQuery { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5;
    }
}