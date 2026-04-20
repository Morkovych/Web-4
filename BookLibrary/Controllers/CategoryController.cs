using BookLibrary.Contracts;
using BookLibrary.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController(
    IBookService bookService,
    ILogger<CategoryController> logger
) : ControllerBase
{
    [HttpGet("statistics")]
    public async Task<ActionResult<ApiResponse<List<CategoryStatisticsResponse>>>> GetCategoryStatistics()
    {
        logger.LogInformation("Fetching category statistics.");

        var statistics = await bookService.GetCategoryStatisticsAsync();

        logger.LogInformation("Successfully returned statistics for {Count} categories.", statistics.Count);

        return Ok(new ApiResponse<List<CategoryStatisticsResponse>>
        {
            Success = true,
            Data = statistics,
            Message = "Category statistics retrieved successfully."
        });
    }
}
