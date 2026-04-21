using BookLibrary.Dto.Requests;
using BookLibrary.Dto.Responses;
using BookLibrary.Models;

namespace BookLibrary.Dto;

public static class BookMappingExtensions
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(
            Id: book.Id,
            Title: book.Title,
            AuthorId: book.AuthorId,
            CategoryId: book.CategoryId,
            Isbn: book.Isbn,
            PublicationYear: book.PublicationYear,
            Genre: book.Genre,
            IsAvailable: book.IsAvailable
        );
    }

    public static BookDetailResponse ToDetailResponse(this Book book)
    {
        return new BookDetailResponse(
            Id: book.Id,
            Title: book.Title,
            Author: book.Author.ToResponse(),
            Category: book.Category.ToResponse(),
            Isbn: book.Isbn,
            PublicationYear: book.PublicationYear,
            Genre: book.Genre,
            IsAvailable: book.IsAvailable
        );
    }

    public static List<BookResponse> ToResponseList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToResponse()).ToList();

    public static List<BookDetailResponse> ToDetailResponseList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToDetailResponse()).ToList();

    public static Book ToBook(this BookRequest request, int id = 0) =>
        new()
        {
            Id = id,
            Title = request.Title,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId,
            Isbn = request.Isbn,
            PublicationYear = request.PublicationYear,
            Genre = request.Genre,
            IsAvailable = request.IsAvailable
        };

    public static AuthorResponse ToResponse(this Author author)
    {
        return new AuthorResponse(
            Id: author.Id,
            FirstName: author.FirstName,
            LastName: author.LastName,
            Bio: author.Bio
        );
    }

    public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse(
            Id: category.Id,
            Name: category.Name,
            Description: category.Description
        );
    }
}