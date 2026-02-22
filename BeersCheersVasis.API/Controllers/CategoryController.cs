using BeersCheersVasis.Api.Models.Category;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoriesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoryAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateCategoryAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.UpdateCategoryAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteCategoryAsync(id, cancellationToken);
        return NoContent();
    }
}
