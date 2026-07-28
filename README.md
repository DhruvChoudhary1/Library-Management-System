# 📚 Library Management System

A web-based **Library Management System** developed using **ASP.NET Core MVC**, **Entity Framework Core**, **SQLite**, **ASP.NET Core Identity**, and **Bootstrap**.

The application provides functionality for managing books, students, librarians, magazines, newspapers, and book borrowing/return operations. It also implements authentication and **role-based authorization** for Admin, Librarian, and User accounts.

---

## 📌 Project Overview

The Library Management System is designed to simplify common library operations through a centralized web application.

The system allows authorized users to manage library resources, maintain student and librarian records, track borrowed and returned books, search records, and control functionality according to user roles.

The project follows the **Model-View-Controller (MVC)** architecture and uses **Entity Framework Core** for database operations.

---

## ✨ Features

### 📚 Book Management

- View all books
- Add new books
- Edit book information
- Delete books
- View book details
- Search by:
  - Title
  - Author
  - Genre
- Pagination
- Availability tracking

---

### 🔄 Borrow & Return Management

- Borrow available books
- Record borrower information
- Automatically mark borrowed books as unavailable
- Return borrowed books
- Automatically restore book availability
- Prevent duplicate returns
- Borrow history tracking
- Validation for unavailable books

---

### 👨‍🎓 Student Management

- Add students
- Edit student information
- Delete students
- Search students
- Pagination
- Role-based access

Student information includes:

- Student ID
- Name
- Email
- Phone

---

### 👨‍💼 Librarian Management

- Add librarian records
- Edit librarian information
- Delete librarians
- Search librarians
- Pagination
- Admin-only management

---

### 📰 Magazine Management

- View magazines
- Add magazines
- Edit magazines
- Delete magazines
- Search magazines
- Pagination
- Availability tracking

---

### 🗞️ Newspaper Management

- View newspapers
- Add newspapers
- Edit newspapers
- Delete newspapers
- Search newspapers
- Pagination
- Availability tracking

---

### 📊 Dashboard

The application contains a dashboard providing access to major library management operations.

Authorized staff can access modules such as:

- Books
- Students
- Borrow History
- Magazines
- Newspapers
- Librarian management

---

## 🔐 Authentication

Authentication is implemented using **ASP.NET Core Identity**.

The system supports:

- User registration
- Login
- Logout
- Secure password hashing
- Authentication cookies
- Unique email accounts
- Role management
- Access denied handling

The original custom login implementation was replaced by ASP.NET Core Identity.

---

## 🛡️ Role-Based Authorization

The system contains three levels of access:

### 👤 User

Normal authenticated users can primarily browse library resources.

### 👨‍💼 Librarian

Librarians can perform day-to-day library operations including:

- Manage books
- Borrow books
- Return books
- View borrow history
- Manage students
- Manage magazines
- Manage newspapers
- Access the dashboard

### 👑 Admin

Administrators have full system access, including:

- All Librarian capabilities
- Delete restricted records
- Manage librarians
- Create Librarian accounts
- Access administrative functionality

---

## 🔑 Permission Matrix

| Feature | User | Librarian | Admin |
|---|:---:|:---:|:---:|
| View Books | ✅ | ✅ | ✅ |
| Add Books | ❌ | ✅ | ✅ |
| Edit Books | ❌ | ✅ | ✅ |
| Delete Books | ❌ | ❌ | ✅ |
| Borrow Books | ❌ | ✅ | ✅ |
| Return Books | ❌ | ✅ | ✅ |
| Borrow History | ❌ | ✅ | ✅ |
| View Students | ❌ | ✅ | ✅ |
| Add Students | ❌ | ✅ | ✅ |
| Edit Students | ❌ | ✅ | ✅ |
| Delete Students | ❌ | ❌ | ✅ |
| View Magazines | ✅ | ✅ | ✅ |
| Add/Edit Magazines | ❌ | ✅ | ✅ |
| Delete Magazines | ❌ | ❌ | ✅ |
| View Newspapers | ✅ | ✅ | ✅ |
| Add/Edit Newspapers | ❌ | ✅ | ✅ |
| Delete Newspapers | ❌ | ❌ | ✅ |
| Dashboard | ❌ | ✅ | ✅ |
| Librarian Management | ❌ | ❌ | ✅ |

---

## 🛠️ Technology Stack

### Backend

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- ASP.NET Core Identity

### Frontend

- HTML5
- CSS3
- Razor Views
- Bootstrap 5

### Database

- SQLite

### Testing

- xUnit
- Entity Framework Core InMemory Database

### Development Tools

- Visual Studio Code / Visual Studio
- .NET CLI
- Git
- GitHub

---

## 🏗️ Architecture

The project follows the **MVC architecture**:

```text
User
  │
  ▼
Browser
  │
  ▼
ASP.NET Core Routing
  │
  ▼
Authentication
  │
  ▼
Authorization
  │
  ▼
Controller
  │
  ├─────────────► ViewModel
  │
  ▼
Entity Framework Core
  │
  ▼
SQLite Database
  │
  ▼
Razor View
  │
  ▼
Browser
```

### Model

Represents application data and validation rules.

Examples:

```text
Book
BorrowRecord
StudentModel
LibrarianModel
Magazine
Newspaper
ApplicationUser
```

### View

Razor views display application data using Bootstrap-based responsive interfaces.

### Controller

Controllers process requests, validate input, interact with the database, enforce authorization, and return views or redirects.

---

## 🗄️ Database Entities

### Book

```text
Id
Title
Author
Genre
PublicationYear
IsAvailable
```

### BorrowRecord

Stores borrowing transactions and tracks whether books have been returned.

### Student

```text
StudentId
StudentName
Email
Phone
```

### Librarian

```text
LibrarianId
Name
Age
Phone
```

### Magazine

```text
Id
Title
Publisher
Category
IssueDate
IsAvailable
```

### Newspaper

```text
Id
Title
Publisher
PublicationDate
IsAvailable
```

### ApplicationUser

Extends ASP.NET Core Identity user functionality and represents authenticated accounts.

---

## 🔗 Book and Borrow Relationship

The primary relationship is:

```text
Book
  │
  │ 1
  │
  ▼
BorrowRecord
  *
```

A book can have multiple borrowing records over time.

An active borrowing record has no return date.

When the book is returned:

```text
ReturnDate = current date/time
```

and:

```text
Book.IsAvailable = true
```

---

## 🔍 Search

Search functionality is available across several modules.

### Books

Search using:

- Title
- Author
- Genre

### Students

Search using:

- Name
- Email
- Phone

### Librarians

Search using:

- Name
- Phone

### Magazines

Search using magazine information such as title, publisher, and category.

### Newspapers

Search using newspaper information such as title and publisher.

---

## 📄 Pagination

Large lists are divided into pages using Entity Framework Core.

Example:

```csharp
var books = await query
    .OrderBy(b => b.Title)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

Pagination information is transferred using ViewModels.

---

## 🧩 ViewModels

The application uses ViewModels for page-specific information.

Examples include:

```text
BookListViewModel
MagazineListViewModel
NewspaperListViewModel
StudentIndexViewModel
LibrarianIndexViewModel
BorrowViewModel
ReturnViewModel
LoginViewModel
RegisterViewModel
```

ViewModels allow lists, search terms, pagination information, and form data to be passed to views without overloading database entities.

---

## ✔️ Validation

ASP.NET Core Data Annotations are used for validation.

Example:

```csharp
[Required(ErrorMessage = "Book title is required.")]
[StringLength(100)]
public string Title { get; set; } = string.Empty;
```

Publication year validation:

```csharp
[Required(ErrorMessage = "Publication year is required.")]
[Range(1000, 3000)]
public int PublicationYear { get; set; }
```

Controllers check:

```csharp
ModelState.IsValid
```

before storing data.

---

## 🔒 Security

The application includes:

- ASP.NET Core Identity
- Password hashing
- Authentication cookies
- Role-based authorization
- Anti-forgery validation
- Server-side access restrictions
- Local ReturnUrl validation
- Access Denied handling

Authorization is enforced using:

```csharp
[Authorize]
```

and:

```csharp
[Authorize(Roles = "Admin,Librarian")]
```

Admin-only functionality uses:

```csharp
[Authorize(Roles = "Admin")]
```

The interface also hides controls that the current user cannot use.

Server-side authorization remains the actual security boundary.

---

## 🧪 Testing

The project includes automated controller tests using **xUnit**.

Tests use the Entity Framework Core InMemory provider to isolate test data from the actual SQLite database.

The BooksController test suite covers operations such as:

- Index
- Create
- Edit
- Delete
- Database changes
- Invalid/not-found scenarios

The established test suite reached:

```text
Total Tests: 19
Passed:      19
Failed:      0
```

Run tests using:

```bash
dotnet test
```

---

## 📁 Project Structure

```text
LibraryManagement/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── BooksController.cs
│   ├── BorrowController.cs
│   ├── DashboardController.cs
│   ├── HomeController.cs
│   ├── LibrarianController.cs
│   ├── MagazineController.cs
│   ├── NewspaperController.cs
│   └── StudentController.cs
│
├── Data/
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── Book.cs
│   ├── BorrowRecord.cs
│   ├── LibrarianModel.cs
│   ├── LibraryContext.cs
│   ├── Magazine.cs
│   ├── Newspaper.cs
│   └── StudentModel.cs
│
├── ViewModels/
│
├── Views/
│   ├── Account/
│   ├── Books/
│   ├── Borrow/
│   ├── Dashboard/
│   ├── Home/
│   ├── Librarian/
│   ├── Magazine/
│   ├── Newspaper/
│   ├── Student/
│   └── Shared/
│
├── Migrations/
├── LibraryManagement.Tests/
├── wwwroot/
│   └── css/
│
├── Program.cs
├── appsettings.json
├── LibraryManagement.csproj
└── README.md
```

---

# 🚀 Running the Project

## Prerequisites

Install:

- .NET 8 SDK
- Git
- A code editor such as Visual Studio or Visual Studio Code

Check your .NET installation:

```bash
dotnet --version
```

---

## 1. Clone the Repository

```bash
git clone https://github.com/YOUR-USERNAME/Library-Management-System.git
```

Move into the project:

```bash
cd Library-Management-System
```

---

## 2. Restore Dependencies

```bash
dotnet restore
```

---

## 3. Apply Database Migrations

```bash
dotnet ef database update
```

If the EF CLI tool is unavailable:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

---

## 4. Build the Application

```bash
dotnet build
```

---

## 5. Run Tests

```bash
dotnet test
```

---

## 6. Start the Application

```bash
dotnet run
```

The terminal will display the local application address.

Open that address in your browser.

---

## 🗃️ Entity Framework Core Migrations

List migrations:

```bash
dotnet ef migrations list
```

Apply migrations:

```bash
dotnet ef database update
```

Create a migration after changing database models:

```bash
dotnet ef migrations add MigrationName
```

---

## 🌱 Seed Data

The application includes seed data for demonstration purposes, including:

- Books
- Students
- Librarians
- Magazines
- Newspapers

Identity initialization also creates required application roles and the configured administrative account.

> Authentication credentials should not be published in the repository README. Configure development credentials securely for your environment.

---

## 📱 Responsive Design

Bootstrap 5 is used to provide:

- Responsive navigation
- Tables
- Forms
- Buttons
- Cards
- Alerts
- Badges
- Pagination
- Mobile-friendly layouts

---

## ⚠️ Error Handling

The system handles cases such as:

- Book not found
- Borrow record not found
- Book unavailable
- Book already returned
- Invalid form input
- Unauthorized access
- Access denied

Success and error messages are displayed through `TempData`.

---

## 🧠 Key Concepts Demonstrated

This project demonstrates:

- MVC architecture
- Object-oriented programming
- Dependency injection
- Relational database design
- Entity relationships
- CRUD operations
- LINQ
- Asynchronous programming
- EF Core migrations
- Seed data
- Authentication
- Authorization
- Role-based access control
- Razor Views
- ViewModels
- Data validation
- Search
- Pagination
- Automated testing
- Git version control

---

## 🔮 Future Enhancements

Potential improvements include:

- Book reservations
- Due dates
- Fine calculation
- Email notifications
- Barcode/QR scanning
- Multiple physical copies of books
- Inventory management
- User borrowing history
- Password reset
- Email verification
- Audit logs
- Advanced dashboard analytics
- Export reports to PDF/Excel
- Cloud deployment
- REST API
- Mobile application integration

---

## 👨‍💻 Author

**Dhruv Choudhary**

B.Tech — Computer Science & Engineering (AI/ML)  
VIT Bhopal University

---

## 📌 Project Status

**Completed**

Core library management, authentication, role-based authorization, database persistence, search, pagination, and automated testing have been implemented.

---

## ⭐ Support

If you find this project useful, consider giving the repository a ⭐ on GitHub.