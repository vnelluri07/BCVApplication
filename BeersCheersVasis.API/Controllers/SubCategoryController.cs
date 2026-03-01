using BeersCheersVasis.Api.Models.SubCategory;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SubCategoryController : ControllerBase
{
    private readonly ISubCategoryService _service;

    public SubCategoryController(ISubCategoryService service)
    {
        _service = service;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(cancellationToken));

    [HttpGet("GetByCategory/{categoryId}")]
    public async Task<IActionResult> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken)
        => Ok(await _service.GetByCategoryAsync(categoryId, cancellationToken));

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
        => Ok(await _service.GetAsync(id, cancellationToken));

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken)
        => Ok(await _service.CreateAsync(request, cancellationToken));

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync(UpdateSubCategoryRequest request, CancellationToken cancellationToken)
        => Ok(await _service.UpdateAsync(request, cancellationToken));

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
