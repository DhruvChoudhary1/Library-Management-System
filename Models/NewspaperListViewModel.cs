using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class NewspaperListViewModel
    {
        public IEnumerable<Newspaper> Newspapers { get; set; } = new List<Newspaper>();

        public string? SearchQuery { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5;
    }
}