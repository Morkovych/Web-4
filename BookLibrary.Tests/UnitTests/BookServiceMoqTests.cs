using BookLibrary.Contracts;
using BookLibrary.Models;
using BookLibrary.Services;
using BookLibrary.Validators;
using FluentAssertions;
using Moq;

namespace BookLibrary.Tests.UnitTests;

public class BookServiceMoqTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly BookService _service;

    private static readonly Author TestAuthor = new()
    {
        Id = 1, FirstName = "Robert", LastName = "Martin", Bio = "Software engineer"
    };

    private static readonly Category TestCategory = new()
    {
        Id = 1, Name = "Technical literature", Description = "Tech books"
    };

    public BookServiceMoqTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _service = new BookService(_repositoryMock.Object, new BookValidator());
    }

    private static Book CreateTestBook(int id = 1, string title = "Clean Code") => new()
    {
        Id = id,
        Title = title,
        AuthorId = 1,
        CategoryId = 1,
        Isbn = "978-11-1111-000-1",
        PublicationYear = 2008,
        Genre = "Technical literature",
        IsAvailable = true,
        Author = TestAuthor,
        Category = TestCategory
    };

    [Fact]
    public async Task GetAllAsync_NoFilters_ReturnsAllBooks()
    {
        var books = new List<Book> { CreateTestBook(1, "Clean Code"), CreateTestBook(2, "Clean Architecture") };
        _repositoryMock.Setup(r => r.GetAllAsync(null, null)).ReturnsAsync(books);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        _repositoryMock.Verify(r => r.GetAllAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthor_ReturnsFilteredBooks()
    {
        var books = new List<Book> { CreateTestBook(1), CreateTestBook(2) };
        _repositoryMock.Setup(r => r.GetAllAsync("Martin", null)).ReturnsAsync(books);

        var result = await _service.GetAllAsync(author: "Martin");

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_FilterByAuthor_NoMatch_ReturnsEmpty()
    {
        _repositoryMock.Setup(r => r.GetAllAsync("NonExistent", null)).ReturnsAsync(new List<Book>());

        var result = await _service.GetAllAsync(author: "NonExistent");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_SortByTitle_ReturnsSorted()
    {
        var books = new List<Book>
        {
            CreateTestBook(1, "Alpha Book"),
            CreateTestBook(2, "Zebra Book")
        };
        _repositoryMock.Setup(r => r.GetAllAsync(null, "title")).ReturnsAsync(books);

        var result = await _service.GetAllAsync(sortBy: "title");

        result.Select(b => b.Title).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsBook()
    {
        var book = CreateTestBook();
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);

        var result = await _service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidBook_CallsRepositoryAdd()
    {
        var book = CreateTestBook(0);
        _repositoryMock.Setup(r => r.AddAsync(book)).Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(book);

        result.Should().NotBeNull();
        _repositoryMock.Verify(r => r.AddAsync(book), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBook_ReturnsTrue()
    {
        var book = CreateTestBook();
        _repositoryMock.Setup(r => r.UpdateAsync(book)).ReturnsAsync(true);

        var result = await _service.UpdateAsync(book);

        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingBook_ReturnsFalse()
    {
        var book = CreateTestBook(999);
        _repositoryMock.Setup(r => r.UpdateAsync(book)).ReturnsAsync(false);

        var result = await _service.UpdateAsync(book);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsTrue()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        var result = await _service.DeleteAsync(999);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllWithDetailsAsync_ReturnsBooks()
    {
        var books = new List<Book> { CreateTestBook() };
        _repositoryMock.Setup(r => r.GetAllWithDetailsAsync(null, null)).ReturnsAsync(books);

        var result = await _service.GetAllWithDetailsAsync();

        result.Should().HaveCount(1);
        _repositoryMock.Verify(r => r.GetAllWithDetailsAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task GetBooksByAuthorIdAsync_ReturnsBooks()
    {
        var books = new List<Book> { CreateTestBook() };
        _repositoryMock.Setup(r => r.GetBooksByAuthorIdAsync(1)).ReturnsAsync(books);

        var result = await _service.GetBooksByAuthorIdAsync(1);

        result.Should().HaveCount(1);
        _repositoryMock.Verify(r => r.GetBooksByAuthorIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCategoryStatisticsAsync_ReturnsStatistics()
    {
        var books = new List<Book>
        {
            CreateTestBook(1, "Book A"),
            CreateTestBook(2, "Book B")
        };
        _repositoryMock.Setup(r => r.GetAllWithDetailsAsync(null, null)).ReturnsAsync(books);

        var result = await _service.GetCategoryStatisticsAsync();

        result.Should().HaveCount(1);
        result[0].CategoryName.Should().Be("Technical literature");
        result[0].BookCount.Should().Be(2);
    }
}
