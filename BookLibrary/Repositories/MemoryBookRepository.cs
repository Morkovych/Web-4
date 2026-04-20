using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Repositories;

public class MemoryBookRepository : IBookRepository
{
    private int _nextId = 1;

    private readonly List<Author> _authors;
    private readonly List<Category> _categories;
    private readonly List<Book> _books;

    public MemoryBookRepository()
    {
        _authors =
        [
            new Author { Id = 1, FirstName = "Robert", LastName = "Martin", Bio = "Software engineer and author" }
        ];

        _categories =
        [
            new Category { Id = 1, Name = "Technical literature", Description = "Books about software development" }
        ];

        _books =
        [
            new Book
            {
                Id = GetNextId(),
                Title = "Clean Code",
                AuthorId = 1,
                CategoryId = 1,
                Isbn = "978-11-1111-000-1",
                PublicationYear = 2008,
                Genre = "Technical literature",
                IsAvailable = true,
                Author = _authors[0],
                Category = _categories[0]
            },

            new Book
            {
                Id = GetNextId(),
                Title = "Clean Architecture",
                AuthorId = 1,
                CategoryId = 1,
                Isbn = "978-11-1111-000-2",
                PublicationYear = 2017,
                Genre = "Technical literature",
                IsAvailable = true,
                Author = _authors[0],
                Category = _categories[0]
            },

            new Book
            {
                Id = GetNextId(),
                Title = "Clean Agile",
                AuthorId = 1,
                CategoryId = 1,
                Isbn = "978-11-1111-000-3",
                PublicationYear = 2019,
                Genre = "Technical literature",
                IsAvailable = true,
                Author = _authors[0],
                Category = _categories[0]
            }
        ];
    }

    public Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null) =>
        Task.FromResult(ApplyFilters(_books, author, sortBy));

    public Task<Book?> GetByIdAsync(int id) => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public Task AddAsync(Book book)
    {
        book.Id = GetNextId();
        _books.Add(book);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(Book book)
    {
        var index = _books.FindIndex(b => b.Id == book.Id);
        if (index == -1)
            return Task.FromResult(false);

        _books[index] = book;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var index = _books.FindIndex(b => b.Id == id);
        if (index == -1)
            return Task.FromResult(false);

        _books.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<List<Book>> GetAllWithDetailsAsync(string? author = null, string? sortBy = null) =>
        Task.FromResult(ApplyFilters(_books, author, sortBy));

    public Task<List<Book>> GetBooksByAuthorIdAsync(int authorId) =>
        Task.FromResult(_books.Where(b => b.AuthorId == authorId).ToList());

    private int GetNextId() => _nextId++;

    private static List<Book> ApplyFilters(List<Book> books, string? author, string? sortBy)
    {
        IEnumerable<Book> result = books;

        if (!string.IsNullOrWhiteSpace(author))
        {
            result = result.Where(b =>
                (b.Author.FirstName + " " + b.Author.LastName)
                .Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        result = string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase)
            ? result.OrderBy(b => b.Title)
            : result.OrderBy(b => b.Id);

        return result.ToList();
    }
}
