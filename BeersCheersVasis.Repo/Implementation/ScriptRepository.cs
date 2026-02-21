using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public class ScriptRepository : IScriptRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public ScriptRepository(IUnitOfWork dbContext)
    {
        _unitOfWork = dbContext;
    }

    public async Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken)
    {
        var scripts = await _dbContext.Script
            .Include(s => s.Category)
            .Where(s => !s.IsDeleted)
            .ToListAsync(cancellationToken);

        return scripts.Select(MapToResponse).ToList();
    }

    public async Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script
            .Include(s => s.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (script is null)
            return new ScriptResponse();

        return MapToResponse(script);
    }

    public async Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        var createdByUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.CreatedBy, cancellationToken);

        if (createdByUser == null)
            throw new ArgumentException($"User '{request.CreatedBy}' not found.");

        var script = new Script
        {
            Title = request.Title,
            Content = request.Content,
            CreatedByUserId = createdByUser.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedByUserId = createdByUser.Id,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            IsPublished = false,
            IsDeleted = false
        };

        _dbContext.Script.Add(script);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(script);
    }

    public async Task<ScriptResponse> UpdateScriptAsync(UpdateScriptRequest request, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new ArgumentException($"Script with ID '{request.Id}' not found.");

        script.Title = request.Title;
        script.Content = request.Content;
        script.ModifiedByUserId = request.ModifiedBy;
        script.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(script);
    }

    public async Task<IEnumerable<ScriptResponse>> GetPublishedScriptsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Script
            .Include(s => s.Category)
            .Where(s => s.IsPublished && !s.IsDeleted)
            .OrderByDescending(s => s.PublishedDate)
            .Select(s => MapToResponse(s))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ScriptResponse>> GetPublishedByCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Script
            .Include(s => s.Category)
            .Where(s => s.IsPublished && !s.IsDeleted && s.CategoryId == categoryId)
            .OrderByDescending(s => s.PublishedDate)
            .Select(s => MapToResponse(s))
            .ToListAsync(cancellationToken);
    }

    public async Task PublishScriptAsync(int id, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new ArgumentException($"Script with ID '{id}' not found.");
        script.IsPublished = true;
        script.PublishedDate = DateTime.UtcNow;
        script.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> PublishAllScriptsAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var unpublished = await _dbContext.Script
            .Where(s => !s.IsPublished && !s.IsDeleted)
            .ToListAsync(cancellationToken);
        foreach (var s in unpublished)
        {
            s.IsPublished = true;
            s.PublishedDate ??= now;
            s.ModifiedDate = now;
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
        return unpublished.Count;
    }

    public async Task UnpublishScriptAsync(int id, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new ArgumentException($"Script with ID '{id}' not found.");
        script.IsPublished = false;
        script.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteScriptAsync(int id, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new ArgumentException($"Script with ID '{id}' not found.");
        script.IsDeleted = true;
        script.IsPublished = false;
        script.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetCategoryAsync(int id, int categoryId, CancellationToken cancellationToken)
    {
        var script = await _dbContext.Script.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new ArgumentException($"Script with ID '{id}' not found.");
        script.CategoryId = categoryId;
        script.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static ScriptResponse MapToResponse(Script s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Content = s.Content,
        CategoryId = s.CategoryId,
        CategoryName = s.Category?.Name,
        IsActive = s.IsActive,
        IsPublished = s.IsPublished,
        IsDeleted = s.IsDeleted,
        PublishedDate = s.PublishedDate,
        CreatedBy = s.CreatedByUserId,
        CreatedDate = s.CreatedDate,
        ModifiedBy = s.ModifiedByUserId,
        ModifiedDate = s.ModifiedDate,
        CommentCount = s.Comments?.Count ?? 0
    };
}
