# Backend Projesi

This project is a backend API built with ASP.NET Core for managing users and roles, featuring user authentication, role-based access control, JWT token generation, and CRUD operations for user data. The project is structured with interfaces and their respective implementations, making it easy to extend and maintain.

## Table of Contents

1. [Features](#features)
2. [Installation](#installation)
3. [Usage](#usage)
4. [API Documentation](#api-documentation)
5. [Contact](#contact)

## Features

- **User Management**: CRUD operations for managing users.
- **Interfaces and Implementations**: The project uses interfaces (`IUserService`) and their implementations (`UserService`) to handle user services.
- **Soft Deletion**: Mark users as inactive without removing them from the database.
- **User Authentication**: Secure user authentication using JWT tokens.
- **Role-Based Access Control**: Restrict access to API endpoints based on user roles.
- **JWT Token Decoding**: Decode JWT tokens to retrieve user information.
- **Password Hashing**: User passwords are securely hashed using ASP.NET Core Identity.
- **AutoMapper Integration**: The `GetAllUsers`, `AddUser` and `UpdateUser` methods use AutoMapper for transparent object-to-object mapping showing only the necessary information.

## Installation

### Prerequisites

- .NET 8.0
- SQL Server (for database)
- Entity Framework Core
- AutoMapper

### Steps to Install

1. Clone the repository:
    ```bash
    git clone https://github.com/mikailakar/Backend-Projesi.git
    ```
2. Open the project with Visual Studio.
3. Restore the dependencies:
    ```bash
    dotnet restore
    ```
4. Update the database:
    ```bash
    update-database
    ```
5. Run the project:
    ```bash
    dotnet run
    ```

## Usage

### Running the Project

To start the application, open the project in Visual Studio and run it.

The API will be accessible at `https://localhost:7066` by default.

### Example API Requests

You can use tools like Swagger, Postman, or any HTTP client to interact with the API.

#### Authenticate a User and Retrieve JWT Token

Send a POST request to the `/api/Users/login` endpoint with the user's email and password to get a Token belonging to the user.

#### Get All Users (Admin Only)

Send a GET request to the `/api/Users` endpoint with a valid JWT token in the `Authorization` header.

## API Documentation

### Authentication

- `POST /api/Users/login`: Authenticate a user and return a JWT token.
  - **Details**: The JWT token contains the user ID, roles, and expiration date.

### User Management

- `GET /api/Users`: Get a list of all users (Admin only).
  - **Details**: Uses AutoMapper to map the data from the database to the response model.
- `GET /api/Users/{id}`: Get a specific user by ID (Admin only).
- `POST /api/Users`: Add a new user (Admin only).
  - **Details**: User data is mapped from the input model to the database model using AutoMapper. Passwords are securely hashed before saving.
- `PUT /api/Users/{id}`: Update an existing user (Admin only).
  - **Details**: Uses AutoMapper to update only the fields that are provided. Passwords are rehashed if updated.
- `DELETE /api/Users/{id}`: Delete a user by ID (Admin only).
- `DELETE /api/Users/soft/{id}`: Soft delete a user by ID (Admin only).
- `GET /api/Users/GetAllUsersOrderByDate`: Retrieve all users ordered by the date of insertion.

### JWT Decoding

- `GET /api/Users/DecodeJwtToken`: Decode a JWT token and retrieve the user ID, roles, and expiration time.

### User and Role Management

- `GET /api/Users/GetUsersWithRoles`: Retrieve all users with their roles.
- `GET /api/Users/GetUserWithRoleById`: Retrieve a user with their roles by user ID.

## Contact

If you have any questions or suggestions, feel free to reach out:

- **Email**: mikailakr42@gmail.com
- **GitHub**: [mikailakar](https://github.com/mikailakar)
