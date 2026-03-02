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
            Author: book.Author,
            Isbn: book.Isbn,
            PublicationYear: book.PublicationYear,
            Genre: book.Genre,
            IsAvailable: book.IsAvailable
        );
    }

    public static List<BookResponse> ToResponseList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToResponse()).ToList();
}