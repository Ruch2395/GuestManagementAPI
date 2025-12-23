# GuestManagementAPI

Lightweight ASP.NET Core API for managing room reservations.

## Prerequisites
- .NET SDK 8.x installed

## How to run - Setup and run instructions
1. Restore dependencies:

   `dotnet restore`

2. Build the project:

   `dotnet build`

3. Run the API from the repository root:

   `dotnet run --project GuestManagementAPI`

4. When running in Development the OpenAPI docs are enabled (see `Program.cs`). OpenAPI/Swagger will be available at the app's OpenAPI endpoint.


## API endpoints
Base path: `/api/reservations`

- Create a reservation
  - `POST /api/reservations`
  - Body (JSON):
    ```json
    {
      "guestName": "Jane Doe",
      "guestEmail": "jane@example.com",
      "roomNumber": "101",
      "checkInDate": "2025-01-01T15:00:00Z",
      "checkOutDate": "2025-01-03T11:00:00Z",
      "numberOfGuests": 2
    }
    ```
  - Returns: `201 Created`

- List reservations
  - `GET /api/reservations`
  - Optional query params: `status`, `roomNumber`, `sortBy` (e.g. `checkInDate`), `order` (`asc` or `desc`)
  - Example: `GET /api/reservations?status=Pending&sortBy=checkInDate&order=desc`

- Get reservation by id
  - `GET /api/reservations/{id}`

- Update reservation
  - `PUT /api/reservations/{id}`
  - Body: same shape as create DTO

- Actions
  - `POST /api/reservations/{id}/checkin` — mark as checked in
  - `POST /api/reservations/{id}/checkout` — mark as checked out
  - `POST /api/reservations/{id}/cancel` — cancel reservation

## Design decisions
- Separation of concerns: code is organized into `Controllers`, `Services`, and `Repositories` to keep web concerns, business logic, and data access isolated.
- `AppDbContext` centralizes EF Core access. The repository (`ReservationRepository`) encapsulates EF queries so services do not depend on EF types directly.
- DTOs (`ReservationDto`) keep the external API contract independent of internal persistence models.
- A small `GlobalExceptionMiddleware` provides uniform error responses and avoids duplicating try/catch across controllers.
- Dependency injection is used throughout (registered in `Program.cs`) to make components testable and replaceable.

## What you'd improve (future work)
- Persistence: Add a real DB provider (SQL Server/Postgres) and migrations via EF Core.
- Validation: Add more robust validation and model-level checks; integrate FluentValidation for richer rules.
- Tests: Add unit tests for services and integration tests for controllers (use `WebApplicationFactory<T>`).
- Mapping: Replace manual mapping with `AutoMapper` to reduce boilerplate.
- Concurrency: Add optimistic concurrency handling (rowversion) and conflict responses.
- Paging & filtering: Add paging for `GET` list endpoints and richer filtering capabilities.
- Security: Add authentication/authorization (JWT) and input sanitization.
- Observability: Add structured logging and metrics, and improve error telemetry.
- API docs: Improve OpenAPI documentation (examples, schemas) and enable Swagger UI for local debugging.
