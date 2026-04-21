using BookLibrary.Dto.Responses;
using BookLibrary.Models;

namespace BookLibrary.Contracts;

public interface IBookService
{
    Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null, bool withDetails = false);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
    Task<List<Book>> GetAllWithDetailsAsync();
    Task<List<Book>> GetBooksByAuthorIdAsync(int authorId);
    Task<List<CategoryStatisticsResponse>> GetCategoryStatisticsAsync();
}