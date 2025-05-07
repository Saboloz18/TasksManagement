# TasksManagement

A task management system with an API and a background worker for task reassignment, built with .NET 8.0 


## Notes
- In assignment it was stated that we should have had Tasks and Users but changed Tasks to Works for clarity, beause it conflicted with Task class.
- Added Authorization and Authentication
- The ADMIN user is seeded on startup of an API. user:Admin password:Admin@123
- Added Unit tests For business logic layer

## Project Architecture

The project follows Clean Architecture with MediatR for CQRS. It is divided into layers:

- **`TasksManagementAPI` and `TasksManagementReassignmentWorker`**: Entry points (API and worker).
- **`TasksManagement.Application`**: Core layer with business logic, MediatR handlers for CQRS (commands/queries).
- **`TasksManagement.Domain`**: Layer defining entities, value objects, and domain rules.
- **`TasksManagement.Infrastructure`**: Cross-cutting concerns (middleware).
- **`TasksManagement.Persistence`**: Data access layer (EF Core, SQL Server).

The API handles client requests, the worker schedules task reassignments, and both interact with the database via the persistence layer, using the application layer for business logic and domain layer for entities.

## Technologies Used

- **.NET 8.0**: Core framework for building the API and worker.
- **Quartz.NET (3.8.0)**: Scheduling library for background task reassignment
- **Entity Framework Core**: ORM for database access 
- **MS SQL Server**: Database for storing tasks and user data.
- **JWT Authentication**: Secures API endpoints 
- **Docker & Docker Compose**: Containerization and orchestration for running services (API, worker, and database).
- **Swagger/OpenAPI**: API documentation and testing interface (available in Development mode).

## Prerequisites

- **Docker Desktop**: Ensure Docker Desktop is installed and running (for Docker instructions).
- **Visual Studio 2022**: For running locally with Visual Studio (supports .NET 8.0).
- **SQL Server**: A local or remote MS SQL Server instance (for Visual Studio setup).
- **Git**: To clone the repository.

## Running with Docker

### 1. Clone the Repository
```bash
git clone https://github.com/Saboloz18/TasksManagement.git
cd TasksManagement
```

### 2. Build and Run
- From the project root (`TasksManagement`), run:
  ```bash
  docker compose up -d
  ```
- This will:
  - Build and start the SQL Server container (`db`).
  - Build and start the API (`TasksManagementAPI`) on `http://localhost:8080`.
  - Build and start the worker (`TasksManagementReassignmentWorker`).

### 3. Access the API
- Open `http://localhost:8080/swagger/index.html` in your browser to access the Swagger UI.
- Use Auth endpoints to Log in with admin user - user:Admin password:Admin@123, copy token and pass it on for later requests, Dont forget "Bearer" in from of token, for example: Bearer {token}
- Use the API endpoints to manage tasks (requires JWT authentication).


## Running with Visual Studio

### 1. Clone the Repository
```bash
git clone https://github.com/Saboloz18/TasksManagement.git
cd TasksManagement
```

### 2. Open the Solution
- Open `TasksManagement.sln` in Visual Studio 2022.

### 3. Update Connection String(Optional)
- Open `TasksManagementAPI\appsettings.Development.json` and `TasksManagementReassignmentWorker\appsettings.Development.json`.
- Update the `DefaultConnection` string which is set to (localdb)\\MSSQLLocalDB by default:
  ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TasksManagement;Trusted_Connection=True;"
  }
  ```
### 4. Run the API
- Set `TasksManagementAPI` as the startup project.
- Press `F5` to run in Debug mode.
- The API will start, and Swagger UI will be available at `https://localhost:8080/swagger`.

### 5. Run the Worker
- Set `TasksManagementReassignmentWorker` as the startup project.
- The worker will start, executing the `TaskReassignmentJob` based on the cron schedule (`0 0/2 * * * ?` by default, every 2 minutes).



