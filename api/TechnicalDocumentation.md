# **RSVP Tracker API - Technical Documentation**

## **1. Overview**

The **RSVP Tracker API** is a .NET-based RESTful API that manages event scheduling and RSVP tracking. The system allows users to browse events, register their attendance, and retrieve event details. The API is built using **ASP.NET Core 8.0** and follows **Clean Architecture** with **Monolith DDD**, ensuring a modular, maintainable, and scalable design.

## **2. Architectural Design**

The application follows **Clean Architecture**, which enforces separation of concerns and promotes testability. The structure is divided into four key layers:

### **2.1 Layers Overview**

- **API Layer (`Rsvp.Api`)**

  - Handles HTTP requests via **Controllers**.
  - Provides **Swagger-based API documentation**.
  - Implements **middleware** for logging and exception handling.

- **Application Layer (`Rsvp.Application`)**

  - Contains **business logic** using **CQRS (Command Query Responsibility Segregation)**.
  - Uses **MediatR** to process commands and queries.
  - Implements **validation rules** for API inputs.

- **Domain Layer (`Rsvp.Domain`)**

  - Defines **business entities** (`Event`, `User`, `Attendee`).
  - Enforces **domain logic** and constraints.

- **Infrastructure Layer (`Rsvp.Infrastructure`)**
  - Implements **Entity Framework Core** for data persistence.
  - Defines **repository implementations**.
  - Manages **database migrations** and **seeding**.

### **2.2 Key Design Patterns**

The API leverages several **design patterns**:

- **CQRS (Command Query Responsibility Segregation)**

  - Commands handle **write** operations (e.g., `SaveRsvpCommand`).
  - Queries handle **read** operations (e.g., `GetEventByIdQuery`).

- **Mediator Pattern**

  - **MediatR** facilitates decoupling between handlers and controllers.

- **Repository Pattern**

  - Ensures abstraction between the domain and database persistence.

- **Middleware for Logging & Exception Handling**
  - Centralized handling of requests, logs, and errors.

## **3. API Endpoints**

This section details the available API endpoints.

### **3.1 Health Check**

| Method | Endpoint      | Description                |
| ------ | ------------- | -------------------------- |
| `GET`  | `/api/health` | Verifies API availability. |

### **3.2 Event Management**

| Method | Endpoint                    | Description                        |
| ------ | --------------------------- | ---------------------------------- |
| `GET`  | `/api/v1/events/{id}`       | Fetches details of an event by ID. |
| `GET`  | `/api/v1/events/search?...` | Searches for events with filters.  |

### **3.3 RSVP Management**

| Method | Endpoint                 | Description                   |
| ------ | ------------------------ | ----------------------------- |
| `POST` | `/api/v1/rsvp/{eventId}` | Submits an RSVP for an event. |

## **4. Core Components and Implementation Details**

### **4.1 Controllers**

Controllers are defined under `/api/Rsvp.Api/Controllers`. Key controllers include:

- **`EventsController.cs`**

  - Handles event-related queries.
  - Uses dependency injection to call `EventsControllerService`.

- **`RsvpController.cs`**
  - Manages RSVP submissions and retrieval.

### **4.2 Application Layer**

Located under `/api/Rsvp.Application`, this layer implements **CQRS** with **MediatR**.

#### **4.2.1 Queries**

- **`GetEventByIdQuery`**
  - Fetches event details based on an event ID.
  - Uses `IEventRepository` for data retrieval.
- **`GetPaginatedEventsQuery`**
  - Retrieves paginated event lists.
  - Filters based on search criteria.

#### **4.2.2 Commands**

- **`SaveRsvpCommand`**
  - Handles RSVP submissions.
  - Uses `IAttendeeRepository` to save user RSVPs.

#### **4.2.3 Validation**

- **`GetEventByIdQueryValidator`**: Ensures valid event ID input.
- **`SaveRsvpCommandValidator`**: Validates RSVP submission data.

### **4.3 Domain Layer**

Located in `/api/Rsvp.Domain`, this layer includes:

- **Entities** (`Event.cs`, `User.cs`, `Attendee.cs`)
- **Enumerations** (`RsvpStatus.cs`, `UserRole.cs`)
- **Interfaces** (`IEventRepository.cs`, `IAttendeeRepository.cs`)

### **4.4 Infrastructure Layer**

The persistence mechanism is implemented in `/api/Rsvp.Infrastructure`.

- **Database Context (`RsvpContext.cs`)**

  - Uses **Entity Framework Core** for data operations.
  - Configures entity relationships.

- **Repositories**

  - `EventRepository.cs` → Implements `IEventRepository`.
  - `AttendeeRepository.cs` → Implements `IAttendeeRepository`.

- **Database Migrations**

  - Located in `/Migrations/`.
  - Manages schema changes.

- **Seeding**
  - `DatabaseInitializer.cs` initializes test data.

## **5. Logging & Exception Handling**

### **5.1 Logging Middleware**

- **`RequestLoggingMiddleware.cs`**
  - Logs API request details for debugging and monitoring.

### **5.2 Global Exception Handling**

- **`GlobalExceptionHandler.cs`**
  - Centralized error handling to standardize API error responses.

## **6. Testing Strategy**

The API is well-tested, using xUnit for unit and integration testing.

### **6.1 Unit Testing**

Located under `/api/Rsvp.Application.Tests/` and `/api/Rsvp.Domain.Tests/`.

- **Tests Cover:**
  - **Queries & Commands**: Validation, business logic, repository interaction.
  - **Controllers**: API request handling.

### **6.2 Integration Testing**

Located under `/api/Rsvp.Integration.Tests/`.

- **End-to-end tests** validate:
  - API request and response behavior.
  - Database interaction correctness.

### **6.3 Shared Testing Utilities**

Located in `/api/Rsvp.Tests.Shared/` to provide reusable JSON datasets.

## **7. Deployment & Configuration**

### **7.1 Configuration Files**

- `appsettings.json` → Stores database and API settings.
- `appsettings.Development.json` → Development-specific settings.

### **7.2 Docker Support**

- `Dockerfile` is provided for containerization.

### **7.3 Environment Variables**

- `ConnectionStrings__DefaultConnection` → Defines database connection.
- `Logging__LogLevel__Default` → Controls log verbosity.

## **8. Trade-offs & Future Improvements**

### **8.1 Design Trade-offs**

| Feature                             | Benefit                                          | Trade-off                                  |
| ----------------------------------- | ------------------------------------------------ | ------------------------------------------ |
| **CQRS**                            | Improves separation of concerns                  | More complexity in command/query handling  |
| **MediatR**                         | Decouples controllers from business logic        | Slight performance overhead                |
| **EF Core with Repository Pattern** | Simplifies data access, promotes maintainability | Potential ORM overhead compared to raw SQL |

### **8.2 Future Enhancements**

1. **Authentication & Authorization**
   - Implement **JWT-based authentication** to secure API endpoints.
2. **Performance Optimization**
   - Introduce **caching** (e.g., Redis) for frequently accessed queries.
3. **Event Creation Feature**
   - Add endpoint to allow users to create events.
4. **Notification System**
   - Send email confirmations for RSVP submissions.
