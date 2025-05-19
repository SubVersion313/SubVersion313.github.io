# El-Harrifa E-Commerce Platform

A course project demonstrating a full-stack e-commerce platform built with ASP.NET Core MVC and MySQL.

## Features
- User Authentication & Authorization
- Product Management
- Shopping Cart System
- Order Processing
- Admin Dashboard
- Responsive Design
- Image Upload & Optimization
- Basic Security Implementation

## Technical Stack
- Backend: ASP.NET Core MVC
- Database: MySQL
- Frontend: HTML, CSS, JavaScript
- Image Processing: ImageSharp
- Logging: Serilog

## Setup Instructions

### Prerequisites
- .NET 7.0 SDK
- MySQL Server
- Visual Studio 2022 or VS Code

### Database Setup
1. Install MySQL Server
2. Create a new database:
   ```sql
   CREATE DATABASE El_Harrifa;
   ```
3. Run the `El_Harrifa.sql` script to create tables

### Application Setup
1. Clone the repository
2. Update connection string in `appsettings.Production.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=El_Harrifa;User=student;Password=course123;"
   }
   ```
3. Install required NuGet packages:
   ```
   Pomelo.EntityFrameworkCore.MySql
   SixLabors.ImageSharp
   Serilog.AspNetCore
   ```

4. Run the application:
   ```
   dotnet run
   ```

## Project Structure
- `/Controllers` - MVC Controllers
- `/Models` - Data Models
- `/Views` - Razor Views
- `/Frontend` - Static Files
- `/Helpers` - Utility Classes
- `/Services` - Background Services

## Course Requirements
- [x] ASP.NET Core MVC Implementation
- [x] Database Design & Implementation
- [x] User Authentication
- [x] CRUD Operations
- [x] Responsive Design
- [x] Basic Security
- [x] Image Processing
- [x] Error Handling

## Security Features
- Password Hashing
- Session Management
- Basic XSS Protection
- SQL Injection Prevention
- File Upload Validation

## Testing
1. Register a new user account
2. Add products to cart
3. Complete checkout process
4. Test admin features

## Course Evaluation Points
- Code Organization
- Database Design
- Security Implementation
- User Interface
- Error Handling
- Documentation

## Notes
- This is a course project for educational purposes
- Basic security measures are implemented
- Some features are simplified for demonstration
- Database is configured for local development

## Author
[Your Name]
Course: [Course Name]
Year: 2024
