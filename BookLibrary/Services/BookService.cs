using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Services;

public class BookService : IBookService
{
    public Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null)
    {
        var books = BookStoreService.GetAll().ToList();

        if (!string.IsNullOrWhiteSpace(author))
        {
            books = books
                .Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase))
        {
            books = books.OrderBy(b => b.Title).ToList();
        }

        return Task.FromResult(books);
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = BookStoreService.GetById(id);
        return Task.FromResult(book);
    }

    public Task<Book> CreateAsync(Book book)
    {
        BookStoreService.Add(book);
        return Task.FromResult(book);
    }

    public Task<bool> UpdateAsync(Book book)
    {
        return Task.FromResult(BookStoreService.Update(book));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(BookStoreService.Delete(id));
    }
}