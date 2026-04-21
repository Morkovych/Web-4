using BookLibrary.Contracts;
using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null)
    {
        var query = context.Books.AsNoTracking().AsQueryable();
        query = ApplyFilters(query, author, sortBy);
        return await query.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }

    public async Task AddAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var existing = await context.Books.FindAsync(book.Id);
        if (existing == null)
            return false;

        existing.Title = book.Title;
        existing.Isbn = book.Isbn;
        existing.PublicationYear = book.PublicationYear;
        existing.Genre = book.Genre;
        existing.IsAvailable = book.IsAvailable;
        existing.AuthorId = book.AuthorId;
        existing.CategoryId = book.CategoryId;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Book>> GetAllWithDetailsAsync(string? author = null, string? sortBy = null)
    {
        var query = context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .AsQueryable();
        query = ApplyFilters(query, author, sortBy);
        return await query.ToListAsync();
    }

    private static IQueryable<Book> ApplyFilters(IQueryable<Book> query, string? author, string? sortBy)
    {
        if (!string.IsNullOrWhiteSpace(author))
        {
            var authorLower = author.ToLower();
            query = query.Where(b =>
                (b.Author.FirstName + " " + b.Author.LastName).ToLower().Contains(authorLower));
        }

        query = string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase)
            ? query.OrderBy(b => b.Title)
            : query.OrderBy(b => b.Id);

        return query;
    }

    public async Task<List<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        return await context.Books
            .AsNoTracking()
            .Where(b => b.AuthorId == authorId)
            .Include(b => b.Author)
            .Include(b => b.Category)
            .OrderBy(b => b.Id)
            .ToListAsync();
    }
}
