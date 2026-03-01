# BCV Application (Personal Project)

Location: `/mnt/c/Vasi Personal/Vasi Code/BCVApp-Final/BCVApplication`

## Stack
- .NET 9, Blazor WebAssembly (`BlazorApp3`) + Web API (`BeersCheersVasis.API`)
- Entity Framework Core with SQL Server
- MudBlazor UI components
- MVVM pattern

## Architecture
```
API → Services → Repository → Data (EF Core SQL Server)
```
- Repository + Unit of Work pattern
- See `development-guidelines.md` for step-by-step checklist when adding entities or endpoints

## Coding Rules
- `readonly` on fields only assigned in constructor
- `sealed` on classes not designed for inheritance
- All I/O methods async with `CancellationToken`
- Constructor injection only
- Return empty collections instead of null

## Build / Test
```bash
dotnet build
dotnet test --verbosity normal
```

## Git (GitHub)
```bash
gh pr create --fill
```
