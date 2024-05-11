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
        try
        {
            var scripts = await _dbContext.Script.ToListAsync(cancellationToken);

            return scripts.Select(s => new ScriptResponse
            {
                Id = s.Id,
                Title = s.Title,
                Content = s.Content,
                IsActive = s.IsActive,
                CreatedBy = s.CreatedByUserId,
                CreatedDate = s.CreatedDate,
                ModifiedBy = s.ModifiedByUserId,
                ModifiedDate = s.ModifiedDate
            }).ToList();
        }
        catch (Exception ex)
        {

            throw ex.InnerException;
        }
    }

    public async Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        // Assuming CreatedBy is the username of the user creating the script
        var createdByUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.CreatedBy, cancellationToken);

        if (createdByUser == null)
        {
            // Handle scenario where the user creating the script is not found
            throw new ArgumentException($"User '{request.CreatedBy}' not found.");
        }

        var script = new Script
        {
            Title = request.Title,
            Content = request.Content,
            CreatedByUserId = createdByUser.Id,
            CreatedDate = DateTime.UtcNow, // Assuming UTC time is used for consistency
            ModifiedByUserId = createdByUser.Id,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true // Assuming the newly created script is active by default
        };

        _dbContext.Script.Add(script);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Mapping the created script to ScriptResponse
        var scriptResponse = new ScriptResponse
        {
            Id = script.Id,
            Title = script.Title,
            Content = script.Content,
            IsActive = script.IsActive,
            CreatedBy = script.CreatedByUserId,
            CreatedDate = script.CreatedDate,
            ModifiedBy = script.ModifiedByUserId,
            ModifiedDate = script.ModifiedDate
        };

        return scriptResponse;
    }


}
