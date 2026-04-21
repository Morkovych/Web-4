using BookLibrary.Contracts;
using BookLibrary.Dto;
using BookLibrary.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/v1/authors")]
public class AuthorController(
    IBookService bookService,
    ILogger<AuthorController> logger
) : ControllerBase
{
    [HttpGet("{id:int}/books")]
    public async Task<ActionResult<ApiResponse<List<BookDetailResponse>>>> GetBooksByAuthor(int id)
    {
        logger.LogInformation("Fetching books for author with ID: {AuthorId}", id);

        var books = await bookService.GetBooksByAuthorIdAsync(id);

        logger.LogInformation("Successfully returned {BookCount} books for author {AuthorId}.", books.Count, id);

        return Ok(new ApiResponse<List<BookDetailResponse>>
        {
            Success = true,
            Data = books.ToDetailResponseList(),
            Message = "Books by author retrieved successfully."
        });
    }
}
