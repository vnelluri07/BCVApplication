using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class AppUserRepository : IAppUserRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public AppUserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.AppUsers
            .FirstOrDefaultAsync(u => u.GoogleId == request.GoogleId, cancellationToken);

        if (existing is not null)
        {
            existing.DisplayName = request.DisplayName;
            existing.Email = request.Email;
            existing.AvatarUrl = request.AvatarUrl;
            existing.ModifiedDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return MapToResponse(existing);
        }

        var entity = new AppUser
        {
            GoogleId = request.GoogleId,
            DisplayName = request.DisplayName,
            Email = request.Email,
            AvatarUrl = request.AvatarUrl,
            Role = "User",
            IsAnonymous = false,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.AppUsers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MapToResponse(entity);
    }

    public async Task<AppUserResponse> CreateAnonymousUserAsync(string displayName, CancellationToken cancellationToken)
    {
        var entity = new AppUser
        {
            DisplayName = displayName,
            Role = "User",
            IsAnonymous = true,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.AppUsers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MapToResponse(entity);
    }

    public async Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return entity is null ? null : MapToResponse(entity);
    }

    private static AppUserResponse MapToResponse(AppUser entity) => new()
    {
        Id = entity.Id,
        DisplayName = entity.DisplayName,
        Email = entity.Email,
        AvatarUrl = entity.AvatarUrl,
        Role = entity.Role,
        IsAnonymous = entity.IsAnonymous,
        IsActive = entity.IsActive,
        CreatedDate = entity.CreatedDate
    };

    public async Task SetRoleAsync(int id, string role, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"AppUser {id} not found");
        entity.Role = role;
        entity.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
