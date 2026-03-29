# Yaksh's Sizzling Hot Products Implementation

A production-grade RESTful API built with .NET 10, designed around Clean Architecture principles to ensure long-term maintainability, testability, and scalability.

100% test coverage of all logic files.

_Note:_ To align with Clean Architecture principles, the input seed files have been moved into the `Infrastructure/Seeding` directory, ensuring that data-loading concerns remain within the infrastructure layer rather than the application or API layers.

---

## Key Design Decisions

### Clean Architecture

The solution is structured around Clean Architecture principles to ensure long‑term maintainability, testability, and clear separation of concerns. Business rules live exclusively in the Application layer, while infrastructure and API concerns remain isolated. This allows the system to evolve without creating tight coupling between layers.

### Handler Pattern (CQRS-Aligned)

All use cases are implemented as MediatR request/response handlers. This keeps controllers thin, centralises business logic, and makes each operation independently testable. It also provides a natural extension point for cross‑cutting concerns such as logging, validation, or caching.

### RESTful Controller Design

Controllers are intentionally thin. They translate HTTP concerns, no business logic resides in a controller.

### Explicit Mapping Between Models and DTOs

Mapping is handled through small, intentional extension methods. This avoids hidden magic from large mapping libraries and ensures that the API contract is explicit, predictable, and easy to review. It also keeps the Application layer free from accidental coupling to infrastructure models.

### Custom JSON Converters for Domain Accuracy

DateOnly and OrderStatus use custom converters to enforce consistent formatting and parsing rules. This ensures that domain concepts are represented accurately in API responses and requests, and avoids ambiguity in date handling or enum interpretation.

### Minimal API Surface

The API exposes only the endpoints required for the use case. This keeps the surface area small, reduces cognitive load, and aligns with the principle of designing for clarity rather than breadth.

---

## Technology Stack

Framework: .Net 10

Language: C#

---

## Running the API

```bash
dotnet restore
dotnet run --project SizzlingHotProductsApi --launch-profile https
```

The API will be available at `https://localhost:7248` by default. Swagger UI is accessible at `/swagger` in development mode.

### Running Tests

```bash
dotnet test
```

---

## API Endpoints

### Products

| Method | Endpoint                | Description      |
| ------ | ----------------------- | ---------------- |
| GET    | `/api/products/get-all` | Get all products |

### Orders

| Method | Endpoint              | Description    |
| ------ | --------------------- | -------------- |
| GET    | `/api/orders/get-all` | Get all orders |

### Hot Products

| Method | Endpoint                            | Description                                     |
| ------ | ----------------------------------- | ----------------------------------------------- |
| GET    | `/api/hot-products/today`           | Get the hottest product for today               |
| GET    | `/api/hot-products/past-three-days` | Get the hottest product for the past 3 days     |
| POST   | `/api/hot-products/date-range`      | Get the hottest product for a custom date range |
