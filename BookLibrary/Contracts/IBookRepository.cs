using BookLibrary.Models;

namespace BookLibrary.Contracts;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null);
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
    Task<List<Book>> GetAllWithDetailsAsync(string? author = null, string? sortBy = null);
    Task<List<Book>> GetBooksByAuthorIdAsync(int authorId);
}