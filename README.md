# Order Management API

## Project Overview

This is a .NET 10 Web API project built using Clean Architecture.

This Project Manages:
- Customers
- Products
- Orders

I have implemented:
- Repository Pattern
- Service Layer
- Strategy Pattern for discounts
- Middleware for Logging and Global Exception Handling
- Unit Testing 

---

## Architecture

The solution has 4 Layers :
- API
- Application
- Domain
- Infrastructure

Each layer has separate responsibility to keep the project clean and maintainable.

---

## Features

### Customer
- Create customer
- Get Customer by id

### Product
- Create product
- Get all products

### Order
- Create order
- Apply discounts
- Get order by id

---

## Technologies Used

- .Net 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- xUnit
- Moq

---

## Setup Instructions

1. Clone the Repository
2. Open Solution in Visual Studio 
3. Update Connection string in appsettings.json
4. Run databse migrations using - update database
5. Run the Project
6. Swagger will be open automatically and Endpoints can be tested there