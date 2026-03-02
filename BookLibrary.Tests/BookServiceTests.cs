using BookLibrary.Models;
using BookLibrary.Services;

namespace BookLibrary.Tests;

public class BookServiceTests
{
    private readonly BookService _service = new();

    [Fact]
    public async Task GetAllAsync_NoFilters_ReturnsAllBooks()
    {
        var result = await _service.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthor_ReturnsOnlyMatchingBooks()
    {
        var result = await _service.GetAllAsync(author: "Martin");

        Assert.NotEmpty(result);
        Assert.All(result, b =>
            Assert.Contains("Martin", b.Author, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthorPartialMatch_ReturnsMatchingBooks()
    {
        var result = await _service.GetAllAsync(author: "rob");

        Assert.NotEmpty(result);
        Assert.All(result, b =>
            Assert.Contains("rob", b.Author, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthorNoMatch_ReturnsEmptyList()
    {
        var result = await _service.GetAllAsync(author: "Tolkien_xyz_not_exist");

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_SortByTitle_ReturnsBooksInAlphabeticalOrder()
    {
        var result = await _service.GetAllAsync(sortBy: "title");

        var titles = result.Select(b => b.Title).ToList();
        var sortedTitles = titles.OrderBy(t => t).ToList();
        Assert.Equal(sortedTitles, titles);
    }

    [Fact]
    public async Task GetAllAsync_SortByUnknownField_ReturnsBooks()
    {
        var result = await _service.GetAllAsync(sortBy: "unknown");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsBook()
    {
        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidBook_ReturnsBookWithAssignedId()
    {
        var book = new Book
        {
            Title = "Test Book",
            Author = "Test Author",
            PublicationYear = 2020,
            IsAvailable = true
        };

        var result = await _service.CreateAsync(book);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task CreateAsync_TwoBooks_AssignsDifferentIds()
    {
        var book1 = new Book { Title = "Book One", Author = "Author A", PublicationYear = 2020, IsAvailable = true };
        var book2 = new Book { Title = "Book Two", Author = "Author B", PublicationYear = 2021, IsAvailable = true };

        var result1 = await _service.CreateAsync(book1);
        var result2 = await _service.CreateAsync(book2);

        Assert.NotEqual(result1.Id, result2.Id);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBook_ReturnsTrue()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "Before Update",
            Author = "Some Author",
            PublicationYear = 2010,
            IsAvailable = true
        });

        var updated = new Book
        {
            Id = created.Id,
            Title = "After Update",
            Author = created.Author,
            PublicationYear = created.PublicationYear,
            IsAvailable = created.IsAvailable
        };
        var result = await _service.UpdateAsync(updated);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingBook_ReturnsFalse()
    {
        var book = new Book
        {
            Id = 99999,
            Title = "Ghost Book",
            Author = "Nobody",
            PublicationYear = 2000,
            IsAvailable = false
        };

        var result = await _service.UpdateAsync(book);

        Assert.False(result);
    }

    // ---------- DeleteAsync ----------

    [Fact]
    public async Task DeleteAsync_ExistingBook_ReturnsTrue()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "To Be Deleted",
            Author = "Some Author",
            PublicationYear = 2015,
            IsAvailable = true
        });

        var result = await _service.DeleteAsync(created.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        var result = await _service.DeleteAsync(99999);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletedBook_CannotBeFoundAfterwards()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "Temporary Book",
            Author = "Some Author",
            PublicationYear = 2018,
            IsAvailable = true
        });

        await _service.DeleteAsync(created.Id);
        var found = await _service.GetByIdAsync(created.Id);

        Assert.Null(found);
    }
}
