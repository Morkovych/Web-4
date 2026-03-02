using BookLibrary.Models;

namespace BookLibrary.Services;

public static class BookStoreService
{
    private static int _nextId = 1;

    private static readonly List<Book> Books =
    [
        new()
        {
            Id = GetNextId(),
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Isbn = "978-11-1111-000-1",
            PublicationYear = 2008,
            Genre = "Technical literature",
            IsAvailable = true
        },

        new()
        {
            Id = GetNextId(),
            Title = "Clean Architecture",
            Author = "Robert C. Martin [sort]",
            Isbn = "978-11-1111-000-2",
            PublicationYear = 2017,
            Genre = "Technical literature",
            IsAvailable = true
        },

        new()
        {
            Id = GetNextId(),
            Title = "Clean Agile",
            Author = "Robert C. Martin",
            Isbn = "978-11-1111-000-3",
            PublicationYear = 2019,
            Genre = "Technical literature",
            IsAvailable = true
        }
    ];

    public static IReadOnlyList<Book> GetAll() => Books.AsReadOnly();

    public static Book? GetById(int id) => Books.FirstOrDefault(b => b.Id == id);

    public static void Add(Book book)
    {
        book.Id = GetNextId();
        Books.Add(book);
    }

    public static bool Update(Book book)
    {
        int index = Books.FindIndex(b => b.Id == book.Id);
        if (index == -1)
            return false;

        Books[index] = book;
        return true;
    }

    public static bool Delete(int id)
    {
        int index = Books.FindIndex(b => b.Id == id);
        if (index == -1)
            return false;

        Books.RemoveAt(index);
        return true;
    }

    private static int GetNextId() => _nextId++;
}