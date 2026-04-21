using BookLibrary.Contracts;
using BookLibrary.Dto.Responses;
using BookLibrary.Models;
using FluentValidation;

namespace BookLibrary.Services;

public class BookService(IBookRepository repository, IValidator<Book> validator) : IBookService
{
    public async Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null, bool withDetails = false)
    {
        return withDetails
            ? await repository.GetAllWithDetailsAsync(author, sortBy)
            : await repository.GetAllAsync(author, sortBy);
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        await validator.ValidateAndThrowAsync(book);
        await repository.AddAsync(book);
        return book;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        await validator.ValidateAndThrowAsync(book);
        return await repository.UpdateAsync(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    public async Task<List<Book>> GetAllWithDetailsAsync()
    {
        return await repository.GetAllWithDetailsAsync();
    }

    public async Task<List<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        return await repository.GetBooksByAuthorIdAsync(authorId);
    }

    public async Task<List<CategoryStatisticsResponse>> GetCategoryStatisticsAsync()
    {
        var books = await repository.GetAllWithDetailsAsync();

        return books
            .GroupBy(b => b.Category.Name)
            .Select(g => new CategoryStatisticsResponse(
                CategoryName: g.Key,
                BookCount: g.Count(),
                AveragePublicationYear: (int)Math.Round(g.Average(b => b.PublicationYear))
            ))
            .ToList();
    }
}