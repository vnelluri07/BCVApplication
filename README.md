# BCVApplication

BeersCheersAndVasis (BCV) — A personal .NET web application for managing scripts and users, built with a Blazor WebAssembly frontend and ASP.NET Core Web API backend.

---

## Codebase Overview

### Scale & Metrics
- **Total C# files**: ~110
- **Solution projects**: 7 (active in .sln)
- **Target framework**: .NET 9.0
- **Architecture**: Layered (Clean Architecture)
- **Database**: SQL Server (hosted, EF Core)
- **Frontend**: Blazor WebAssembly + MudBlazor

### Project Breakdown

| Project | Type | Description |
|---|---|---|
| `BeersCheersVasis.API` | ASP.NET Core Web API | REST API with versioning, Swagger, JWT auth config |
| `BeersCheersVasis.Services` | Class Library | Business logic layer (IScriptService, IUserService) |
| `BeersCheersVasis.Repo` | Class Library | Repository + UnitOfWork pattern over EF Core |
| `BeersCheersVasis.Data` | Class Library | EF Core DbContext, entity definitions (Script, User) |
| `BeersCheersVasis.Api.Models` | Class Library | Shared DTOs (request/response models) |
| `BeersCheersVasis.Api.Client` | Class Library | Typed HTTP client for Blazor → API communication |
| `BlazorApp3` | Blazor WebAssembly | Active frontend (MudBlazor UI, MVVM pattern) |

**Legacy/Iteration projects** (not in active .sln): `BlazorApp1`, `BlazorApp2`, `BeersCheersAndVasis.UI`

### Architecture

```
BlazorApp3 (Blazor WASM)
  ├── Pages/ViewModels (MVVM)
  ├── Api.Client (BcvHttpClient)
  │     ↓ HTTP
BeersCheersVasis.API
  ├── Controllers (ScriptController, UserController)
  ├── Internal (DI, CORS, Swagger, JWT config)
  │     ↓
BeersCheersVasis.Services
  ├── ScriptService, UserService
  │     ↓
BeersCheersVasis.Repo
  ├── ScriptRepository, UserRepository
  ├── UnitOfWork (BcvUnitOfWork)
  │     ↓
BeersCheersVasis.Data
  ├── DbContext (EF Core → SQL Server)
  ├── Entities: Script, User
```

### Critical Entry Points

**API**
- `BeersCheersVasis.API/Program.cs` — Host builder, uses Startup class
- `BeersCheersVasis.API/Startup.cs` — DI registration, middleware pipeline
- `BeersCheersVasis.API/Internal/ConfigureApiService.cs` — Service/repo DI wiring
- `BeersCheersVasis.API/Internal/ServiceExtensions.cs` — CORS, JWT, Swagger, API versioning

**Frontend**
- `BlazorApp3/Program.cs` — WASM host, MudBlazor setup, API client DI
- `BlazorApp3/Pages/Script/` — Script CRUD pages
- `BlazorApp3/Pages/User/` — User management page

**Data**
- `BeersCheersVasis.Data/Context/IDbContext.cs` — DbContext interface (Users, Script DbSets)
- `BeersCheersVasis.Data/Entities/` — Script.cs, User.cs

### Technology Stack
- .NET 9.0 (API targets net9.0, packages are 8.0.x)
- ASP.NET Core Web API with Startup class pattern
- Blazor WebAssembly (standalone, not hosted)
- Entity Framework Core 8.0.1 + SQL Server
- MudBlazor 6.19.1 (Material Design components)
- JWT Bearer Authentication (configured, auth middleware commented out)
- API Versioning + Swagger/OpenAPI
- Newtonsoft.Json for serialization in HTTP client

### Key Patterns
- Repository + UnitOfWork
- MVVM in Blazor (ViewModels + code-behind)
- Interface-driven DI (all services/repos have interfaces)
- Typed HTTP client (BcvHttpClient) with custom exception types
- Separate API models (DTOs) from data entities
