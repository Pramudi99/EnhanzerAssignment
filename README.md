# Enhanzer Assignment

A full-stack Purchase Bill web application developed using Angular, ASP.NET Core Web API, and SQL Server.

The application provides secure user authentication through the provided external authentication API and allows authenticated users to create and manage Purchase Bill entries.

---

## Live Demo

**Frontend:**  
https://enhanzer-assignment-lilac.vercel.app/

**Backend API:**  
https://enhanzer-assignment-api-d5dfasgpcncve4c4.southeastasia-01.azurewebsites.net/

---

## GitHub Repository

https://github.com/Pramudi99/EnhanzerAssignment

---

## Technologies Used

### Frontend
- Angular
- TypeScript
- HTML5
- CSS3
- Reactive Forms
- Angular Router
- HTTP Client

### Backend
- ASP.NET Core Web API
- C#
- Entity Framework Core
- JWT Authentication
- REST APIs

### Database
- Microsoft SQL Server

### Deployment
- Frontend: Vercel
- Backend: Microsoft Azure App Service
- Database: Azure SQL Database

---

## Project Structure

```text
EnhanzerAssignment/
│
├── EnhanzerAssignment.API/
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Models/
│   ├── Services/
│   ├── Migrations/
│   ├── Program.cs
│   └── appsettings.json
│
├── Frontend/
│   ├── src/
│   │   └── app/
│   │       ├── core/
│   │       ├── features/
│   │       └── ...
│   ├── angular.json
│   ├── package.json
│   └── vercel.json
│
├── Database/
│   └── EnhanzerAssignmentDb.sql
│
└── README.md
Application Features
1. User Login

The login page allows users to authenticate using:

Email
Password

The credentials are sent to the ASP.NET Core Web API.

The backend communicates with the provided external authentication API:

https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke

The external API is used to validate the user's credentials.

After successful authentication:

User information is received.
User locations are retrieved.
Locations are stored in SQL Server.
A JWT token is generated.
The JWT token is returned to the Angular application.
The authenticated user can access the Purchase Bill page.
2. JWT Authentication

JWT is used to maintain the authenticated session.

The authentication flow is:

Angular Login
      ↓
ASP.NET Core Auth API
      ↓
External Authentication API
      ↓
Authentication Successful
      ↓
Save User Locations
      ↓
Generate JWT
      ↓
Return JWT to Angular
      ↓
Angular Authentication Guard
      ↓
Purchase Bill Page

The backend validates JWT tokens before allowing access to protected API endpoints.

3. Protected Purchase Bill Page

The Purchase Bill page is protected using Angular route guards.

Unauthenticated users cannot directly access the Purchase Bill page.

The application checks whether a valid authentication token exists before allowing navigation to the protected page.

4. Location Management

The locations returned from the external authentication API are stored in SQL Server.

The database contains the LocationDetails table.

The Angular application retrieves the locations through the backend Location API.

The locations are displayed in the Batch dropdown on the Purchase Bill page.

5. Purchase Bill

Authenticated users can create Purchase Bill entries using the following fields:

Item
Batch
Standard Cost
Standard Price
Quantity
Discount
Available Items

The application supports the following items:

Mango
Apple
Banana
Orange
Grapes
Kiwi
Strawberry

The Item field provides autocomplete functionality.

6. Purchase Bill Calculation

The application calculates:

Total Cost
Total Selling
Total Items
Total Quantity

For example:

Standard Cost  = 100
Standard Price = 150
Quantity       = 5
Discount       = 20%

The calculated values are:

Total Cost     = 400
Total Selling  = 750

The Purchase Bill table also displays the added items and the summary information.

Backend API

The ASP.NET Core Web API provides endpoints for authentication and location retrieval.

Authentication
POST /api/Auth/login
Locations
GET /api/Location

The Location endpoint requires authentication.

Environment Configuration

The Angular application uses separate environment configurations for development and production.

Development
http://localhost:5135/api
Production
https://enhanzer-assignment-api-d5dfasgpcncve4c4.southeastasia-01.azurewebsites.net/api

Angular uses the development environment when running locally and replaces it with the production environment during a production build.

Local Development Setup
Prerequisites

Install the following:

Node.js
Angular CLI
.NET 8 SDK
SQL Server / SQL Server Express / LocalDB
SQL Server Management Studio (SSMS)
Git
Backend Setup

Navigate to the API project:

cd EnhanzerAssignment.API

Restore dependencies:

dotnet restore

Build the project:

dotnet build
Database Configuration

Update the SQL Server connection string in:

EnhanzerAssignment.API/appsettings.json

Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EnhanzerAssignmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

The SQL database script is available at:

Database/EnhanzerAssignmentDb.sql

Run the script using SQL Server Management Studio if the database needs to be created manually.

JWT Configuration

The JWT signing key should not be committed to GitHub.

For local development, configure the JWT key using .NET User Secrets.

Initialize User Secrets:

dotnet user-secrets init

Set the JWT key:

dotnet user-secrets set "Jwt:Key" "YOUR_SECRET_KEY"

Other JWT settings are configured in appsettings.json.

Run the Backend
dotnet run

The API will be available at the configured local API URL.

Frontend Setup

Navigate to the frontend:

cd Frontend

Install dependencies:

npm install

Run the Angular development server:

ng serve

Open:

http://localhost:4200
Production Build

To create a production build:

ng build

The production configuration automatically uses:

environment.prod.ts

for the Azure API URL.

Deployment
Frontend

The Angular frontend is deployed using Vercel.

Production URL:

https://enhanzer-assignment-lilac.vercel.app/
Backend

The ASP.NET Core Web API is deployed using Microsoft Azure App Service.

Database

The production database is hosted using Azure SQL Database.

Security

The application includes:

JWT-based authentication
Protected backend API endpoints
Angular route protection
Environment-specific API configuration
JWT secret stored outside source control
CORS configuration for authorized frontend origins
Server-side JWT validation

Sensitive credentials and secrets should not be committed to the repository.

Error Handling

The application provides meaningful error messages for common situations including:

Invalid login credentials
Missing login fields
Authentication service errors
Database errors
Invalid authentication responses
Unauthorized API requests
Testing the Application

Use the following flow to test the application:

1. Open the Live Demo
2. Enter valid login credentials
3. Login successfully
4. Navigate to Purchase Bill
5. Select an Item
6. Select a Batch/Location
7. Enter Standard Cost
8. Enter Standard Price
9. Enter Quantity
10. Enter Discount
11. Click Add
12. Verify the calculated totals
13. Verify Total Items
14. Verify Total Quantity
Database

The SQL Server database script is available here:

Database/EnhanzerAssignmentDb.sql

It can be used to recreate the database structure required by the application.

Author

Pramudi

Full Stack Developer - Angular & .NET Core Assignment

License

This project was developed as part of a Full Stack Developer - Angular & .NET Core Assignment.
