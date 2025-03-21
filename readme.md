# Simple Employee Management Application

This project is a simple ASP.NET Core MVC application that implements basic employee management functionality, including:

- **Login:** Secure user authentication.
- **Employee Listing:** Displaying a list of employees.
- **Employee Creation:** Adding new employees.
- **Employee Editing:** Modifying existing employee details.
- **Employee Deletion (Soft Delete):** Suspending employee accounts instead of permanently deleting them.
- **Live Search:** Real-time search functionality to filter employees by name, position, or other details as the user types in the search bar.

## Core Functionality and Implementation Details

### Login:

- **Implemented using ASP.NET Core's session state.**
- **Secure Password Handling:** Uses BCrypt.Net-Next to verify the entered password against the stored, hashed password. The `AuthenticateUser` stored procedure was removed in favor of performing the hash comparison in C#, which is more secure as it avoids sending the plain text password to the database server.
- **Session Management:** Upon successful login, a session variable (`IsLoggedIn`) is set to `true`, and the user's ID is stored. The `LoginService` provides `IsLoggedIn()` and `GetLoggedInUserId()` methods to check authentication status and retrieve the user ID.
- **Logout:** Clears the session, effectively logging the user out.
- **Back Button Prevention:** Uses the `[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]` attribute on controller actions to prevent the browser from caching sensitive pages (login, employee list, edit, etc.). This ensures that the user is redirected to the login page if they try to use the back button after logging out.

### Employee CRUD Operations:

- **Create, Read, Update, Delete:** Implemented using standard ASP.NET Core MVC patterns (controllers, views, models, and services).
- **Stored Procedures:** All database interactions for employee management are handled through stored procedures (`AddEmployee`, `UpdateEmployee`, `suspendEmployee`, `GetEmployeeById`, `GetAllEmployees`). This enhances security (by mitigating SQL injection risks) and improves maintainability.
- **Soft Delete:** Instead of permanently deleting employee records, the `suspendEmployee` stored procedure sets the `IsActive` flag to `0`, preserving the data.
- **Model Binding and Validation:** Uses ASP.NET Core's model binding and validation features (data annotations and `ModelState.IsValid`) to ensure data integrity.
- **Concurrency Handling:** The Update (Edit) employee functionality includes basic concurrency conflict detection.
- **Dependency Injection:** Services (`LoginService`, `EmployeeService`) and `IHttpContextAccessor` are injected into controllers using ASP.NET Core's built-in dependency injection container.
- **Routing:** Uses standard ASP.NET Core MVC routing.
- **Views:** Uses Razor views to render the user interface. Includes a logout link that's conditionally displayed based on the user's login status.
- **Live Search:** A live search bar is included above the employee list to enable users to filter employees by typing. As users type, the list of employees is dynamically filtered in real-time, improving the usability and search experience. This feature is implemented using JavaScript and provides immediate feedback as users enter search terms.

## Technologies Used

- **ASP.NET Core MVC:** The web framework used for building the application.
- **Entity Framework Core:** An ORM (Object-Relational Mapper) used for database interactions (Code-First approach).
- **SQL Server:** The database used to store employee and user data.
- **BCrypt.Net-Next:** A library used for secure password hashing.
- **Bootstrap:** A CSS framework used for basic styling and layout (implied by the use of `btn`, `table`, etc. classes).
- **C#:** The programming language.
- **.NET:** The development platform.
- **JavaScript:** Used to implement the live search functionality for the employee list.

## Key Security Considerations

- **Password Hashing (BCrypt):** Passwords are never stored in plain text. BCrypt is used for secure, one-way hashing.
- **Session Management:** Uses secure session handling to track user login status.
- **Cache Control:** Prevents caching of sensitive pages to avoid information disclosure via the browser's back button.
- **Stored Procedures:** Mitigates SQL injection vulnerabilities by using parameterized queries.
- **CSRF Protection:** Uses `[ValidateAntiForgeryToken]` on POST actions to prevent Cross-Site Request Forgery attacks.
- **Input Validation:** Uses model validation to ensure data integrity and prevent common vulnerabilities.

## Project Setup and Database Initialization

### Installed Packages

The following NuGet packages were installed for this project:

- **BCrypt.Net-Next (4.0.3):** Used for secure password hashing.
- **Microsoft.EntityFrameworkCore (9.0.3):** The core Entity Framework package.
- **Microsoft.EntityFrameworkCore.Design (9.0.3):** Provides design-time tools for Entity Framework Core.
- **Microsoft.EntityFrameworkCore.SqlServer (9.0.3):** SQL Server database provider for Entity Framework Core.
- **Microsoft.EntityFrameworkCore.Tools (9.0.3):** Tools for Entity Framework Core, including the EF Core CLI.


## Code-First Database Creation

The database schema was created using Entity Framework Core's Code-First approach. `DbContext` and model classes (`Employee`, `User`, `LoginViewModel`) define the structure, and migrations were used to generate and apply the database schema.

### Database Configuration

Before running the application, you need to update the connection string to point to your database.

1. **Update the Connection String**:
   - Open the `appsettings.json` file in the root of your project.
   - Locate the `"ConnectionStrings"` section and update the `DefaultConnection` value with your own database connection string.

   Example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=your_database;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. **Add a Migration**:
   - Open the **Package Manager Console** in Visual Studio.
   - Run the following command to add a new migration:
   ```bash
   Add-Migration InitialCreate
   ```

3. **Update the Database**:
   - After adding the migration, run the following command to apply the changes to your database:
   ```bash
   Update-Database
   ```

This will create or update the database schema based on your current model definitions.

### Initial User Data (Hashed Password)

Because the assignment focused on the _login_ page and not user registration, a secure password for the initial user was generated using BCrypt. This was done with a temporary C# code snippet:

```csharp
using BCrypt.Net;

string password = "admin";
string salt = BCrypt.Net.BCrypt.GenerateSalt();
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
Console.WriteLine($"Hashed Password: {hashedPassword}");
```

The resulting `hashedPassword` string was then _manually_ inserted into the `Users` table's `Password` column (which was altered to `NVARCHAR(255)` to accommodate the BCrypt hash). This simulates a user having registered with a secure password. _This is a crucial security step._

### Database Scripts

The following SQL scripts were implemented:

```sql
-- Get All Employees (View)
CREATE VIEW GetAllEmployees AS
SELECT *
FROM Employees;

-- Add Employee
CREATE PROCEDURE AddEmployee
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Position NVARCHAR(100),
    @Email NVARCHAR(255),
    @Salary DECIMAL(18, 2),
    @DateOfJoining DATE,
    @IsActive BIT,
AS
BEGIN
    INSERT INTO Employees (FirstName, LastName, Position, Email, Salary, DateOfJoining, IsActive)
    VALUES (@FirstName, @LastName, @Position, @Email, @Salary, @DateOfJoining, @IsActive);
END
GO

-- Update Employee
CREATE PROCEDURE UpdateEmployee
    @Id INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Position NVARCHAR(100),
    @Email NVARCHAR(255),
    @Salary DECIMAL(18, 2),
    @DateOfJoining DATE,
    @IsActive BIT
AS
BEGIN
    UPDATE Employees
    SET
        FirstName = @FirstName,
        LastName = @LastName,
        Position = @Position,
        Email = @Email,
        Salary = @Salary,
        DateOfJoining = @DateOfJoining,
        IsActive = @IsActive
    WHERE Id = @Id;
END
GO

-- Soft Delete (Deactivate) Employee
CREATE PROCEDURE suspendEmployee
    @Id INT
AS
BEGIN
    UPDATE Employees
    SET IsActive = 0
    WHERE Id = @Id;
END
GO

-- Get Employee by ID
CREATE PROCEDURE GetEmployeeById
    @Id INT
AS
BEGIN
    SELECT * FROM Employees where Id = @Id
END

-- Authenticate User
CREATE PROCEDURE AuthenticateUser
    @UserName NVARCHAR(50),
    @Password NVARCHAR(100),
    @ReturnValue INT OUTPUT
AS
BEGIN
    DECLARE @UserId INT;

    SELECT @UserId = Id
    FROM Users
    WHERE UserName = @UserName AND Password = @Password;

    IF @UserId IS NOT NULL
    BEGIN
        SET @ReturnValue = 1; -- Valid login
    END
    ELSE
    BEGIN
        SET @ReturnValue = -1; -- Invalid login
    END
END
```
