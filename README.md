# Firefin API

Backend for Firefin — a single ASP.NET Core Web API project (C#, .NET 8, EF Core, SQL Server).

## Why this exists first

Firefin is at Pre-Stage 0: the priority is **mastering the product** (right now, the
Blue Flame sauce) and standing up the commerce foundation. This API's first slice is
therefore two layers over one shared backbone:

- **Catalog** — the product identity (meal / sauce / Fire Drop) that becomes sellable once locked.
- **R&D Lab** — recipes (gram-weight formulas), batches, and structured batch notes that let us
  iterate a product toward a locked, repeatable recipe.

## Architecture

One project, flat and obvious:

```
Firefin.Api/
  Controllers/   thin HTTP endpoints
  Services/      business logic (interface + implementation per concern)
  Models/        EF entities + enums
  DTOs/          request/response shapes
  Data/          DbContext + seeder
  Mapping/       entity -> DTO projections
Firefin.Api.Tests/   xUnit, EF Core InMemory (separate project, same solution)
```

Conventions: component-based, aim for < 500 lines per file; controllers stay thin and
delegate to services; entities never leak past a controller — everything crosses the
boundary as a DTO.

## Data model

`Product` → `Recipe` (versioned) → `RecipeIngredient` (grams) and `Batch` → `BatchNote`.

- A product accumulates recipe **versions**; exactly one is `IsCurrent`.
- A recipe accumulates **batches** (auto-numbered); each batch carries a rating, verdict,
  and categorized notes (Moisture, Heat, Adhesion, CheeseMelt, Separation, Cook, Texture,
  Packaging, Flavor, Other) recording what worked and what didn't.

Seed data creates **Blue Flame** as a `Sauce` in `InDevelopment` with a v1 recipe and batch #1.

## Getting started

Requires the .NET SDK (9.x is fine; the projects target `net8.0`).

```bash
# 1. Point the connection string at your SQL Server instance
#    (edit ConnectionStrings:Firefin in Firefin.Api/appsettings.json)

# 2. Create the initial migration (code-first) and the database
dotnet tool install --global dotnet-ef        # once, if not installed
dotnet ef migrations add InitialCreate --project Firefin.Api
dotnet ef database update --project Firefin.Api

# 3. Run — Swagger UI is at the app URL /swagger in Development
dotnet run --project Firefin.Api

# 4. Tests
dotnet test
```

The app also calls `Database.Migrate()` and seeds on startup, so once a migration exists
the database is created/updated automatically on run.

## API surface (first slice)

| Method | Route | Purpose |
|---|---|---|
| GET | `/api/products` | List all products |
| GET | `/api/products/{id}` | Get product by id |
| GET | `/api/products/slug/{slug}` | Get product by slug |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |
| GET | `/api/products/{productId}/recipes` | Recipe versions for a product |
| POST | `/api/products/{productId}/recipes` | Add a recipe version (becomes current) |
| GET | `/api/recipes/{recipeId}` | Get a recipe |
| GET | `/api/recipes/{recipeId}/batches` | Batches for a recipe |
| POST | `/api/recipes/{recipeId}/batches` | Add a batch (auto-numbered) |
| GET | `/api/batches/{batchId}` | Get a batch |
