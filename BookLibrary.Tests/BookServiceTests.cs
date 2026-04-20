using BookLibrary.Models;
using BookLibrary.Repositories;
using BookLibrary.Services;
using BookLibrary.Validators;
using FluentAssertions;

namespace BookLibrary.Tests;

public class BookServiceTests
{
    private readonly BookService _service = new(new MemoryBookRepository(), new BookValidator());

    [Fact]
    public async Task GetAllAsync_NoFilters_ReturnsAllBooks()
    {
        var result = await _service.GetAllAsync();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthorPartialMatch_ReturnsMatchingBooks()
    {
        var result = await _service.GetAllAsync(author: "Martin");

        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(b =>
            (b.Author.FirstName + " " + b.Author.LastName).ToLowerInvariant().Should().Contain("martin"));
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthorNoMatch_ReturnsEmptyList()
    {
        var result = await _service.GetAllAsync(author: "Tolkien_xyz_not_exist");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_SortByTitle_ReturnsBooksInAlphabeticalOrder()
    {
        var result = await _service.GetAllAsync(sortBy: "title");

        var titles = result.Select(b => b.Title).ToList();
        var sortedTitles = titles.OrderBy(t => t).ToList();

        titles.Should().Equal(sortedTitles);
    }

    [Fact]
    public async Task GetAllAsync_SortByUnknownField_ReturnsBooks()
    {
        var result = await _service.GetAllAsync(sortBy: "unknown");

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsBook()
    {
        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidBook_ReturnsBookWithAssignedId()
    {
        var book = new Book
        {
            Title = "Test Book",
            AuthorId = 1,
            CategoryId = 1,
            PublicationYear = 2020,
            IsAvailable = true
        };

        var result = await _service.CreateAsync(book);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be("Test Book");
    }

    [Fact]
    public async Task CreateAsync_TwoBooks_AssignsDifferentIds()
    {
        var book1 = new Book { Title = "Book One", AuthorId = 1, CategoryId = 1, PublicationYear = 2020, IsAvailable = true };
        var book2 = new Book { Title = "Book Two", AuthorId = 1, CategoryId = 1, PublicationYear = 2021, IsAvailable = true };

        var result1 = await _service.CreateAsync(book1);
        var result2 = await _service.CreateAsync(book2);

        result1.Id.Should().NotBe(result2.Id);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBook_ReturnsTrue()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "Before Update",
            AuthorId = 1,
            CategoryId = 1,
            PublicationYear = 2010,
            IsAvailable = true
        });

        var updated = new Book
        {
            Id = created.Id,
            Title = "After Update",
            AuthorId = created.AuthorId,
            CategoryId = created.CategoryId,
            PublicationYear = created.PublicationYear,
            IsAvailable = created.IsAvailable
        };

        bool result = await _service.UpdateAsync(updated);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_NonExistingBook_ReturnsFalse()
    {
        var book = new Book
        {
            Id = 99999,
            Title = "Ghost Book",
            AuthorId = 1,
            CategoryId = 1,
            PublicationYear = 2000,
            IsAvailable = false
        };

        bool result = await _service.UpdateAsync(book);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ExistingBook_ReturnsTrue()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "To Be Deleted",
            AuthorId = 1,
            CategoryId = 1,
            PublicationYear = 2015,
            IsAvailable = true
        });

        bool result = await _service.DeleteAsync(created.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        bool result = await _service.DeleteAsync(99999);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_DeletedBook_CannotBeFoundAfterwards()
    {
        var created = await _service.CreateAsync(new Book
        {
            Title = "Temporary Book",
            AuthorId = 1,
            CategoryId = 1,
            PublicationYear = 2018,
            IsAvailable = true
        });

        await _service.DeleteAsync(created.Id);
        var found = await _service.GetByIdAsync(created.Id);

        found.Should().BeNull();
    }
}