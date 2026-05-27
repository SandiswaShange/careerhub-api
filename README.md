# CareerHub API

A simple ASP.NET Core Web API for managing job listings.  
This project curretly uses an in-memory data store and is built for learning backend basic API development, but will will soon be a fully functional conference room booking system.

---

## Features

- Get all job listings
- Get a single job listing by ID
- In-memory data storage (no database required)
- API documentation via Scalar

---

## Technologies Used

- ASP.NET Core Web API (.NET 10)
- C#
- Dependency Injection (DI)
- Scalar (OpenAPI documentation)
- Postman (for API testing)

---

## Project Structure

API/
│
├── Controllers/
│    JobsController.cs
│
├── Data/
│    ListingStore.cs
│
├── Models/
│    JobListing.cs
│
└── Program.cs
## Controllers vs Minimal APIs

This project uses Controllers instead of Minimal APIs. The main reason is to improve structure, readability, and scalability as the application grows.

Controllers provide a clear separation of concerns by grouping related endpoints into a single class, which was JobsController in this instance. This makes the code easier to maintain and extend, especially when multiple operations are involved.

While Minimal APIs are faster to set up, I chose controllers because this project is designed as a learning exercise for real-world backend structure, where maintainability and clarity are more important than just being quick and simple.