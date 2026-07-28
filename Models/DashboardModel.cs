namespace LibraryManagement.Models
{
    public class DashboardModel
    {
        public int TotalBooks { get; set; }

        public int AvailableBooks { get; set; }

        public int BorrowedBooks { get; set; }

        public int TotalStudents { get; set; }

        public int TotalLibrarians { get; set; }
    }
}