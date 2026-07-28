using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; } = new List<Book>();

        public string? SearchQuery { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5;
    }
}