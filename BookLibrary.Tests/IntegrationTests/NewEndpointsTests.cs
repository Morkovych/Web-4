using System.Text.Json;
using BookLibrary.Data;
using BookLibrary.Dto.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BookLibrary.Tests.IntegrationTests;

public class NewEndpointsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("password")
        .Build();

    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private int _robertMartinAuthorId;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(_postgres.GetConnectionString()));
            });
        });

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        _robertMartinAuthorId = await SeedTestData(db);

        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    private static async Task<int> SeedTestData(ApplicationDbContext db)
    {
        db.Books.RemoveRange(db.Books);
        db.Categories.RemoveRange(db.Categories);
        db.Authors.RemoveRange(db.Authors);
        await db.SaveChangesAsync();

        var author1 = new Models.Author
        {
            FirstName = "Robert",
            LastName = "Martin",
            Bio = "Software engineer and author"
        };
        var author2 = new Models.Author
        {
            FirstName = "Martin",
            LastName = "Fowler",
            Bio = "Software developer"
        };

        db.Authors.AddRange(author1, author2);
        await db.SaveChangesAsync();

        var category1 = new Models.Category
        {
            Name = "Technical literature",
            Description = "Books about software development"
        };
        var category2 = new Models.Category
        {
            Name = "Science Fiction",
            Description = "Sci-fi books"
        };

        db.Categories.AddRange(category1, category2);
        await db.SaveChangesAsync();

        db.Books.AddRange(
            new Models.Book
            {
                Title = "Clean Code",
                Isbn = "978-11-1111-000-1",
                PublicationYear = 2008,
                Genre = "Technical literature",
                IsAvailable = true,
                AuthorId = author1.Id,
                CategoryId = category1.Id
            },
            new Models.Book
            {
                Title = "Clean Architecture",
                Isbn = "978-11-1111-000-2",
                PublicationYear = 2017,
                Genre = "Technical literature",
                IsAvailable = true,
                AuthorId = author1.Id,
                CategoryId = category1.Id
            },
            new Models.Book
            {
                Title = "Refactoring",
                Isbn = "978-11-1111-000-3",
                PublicationYear = 1999,
                Genre = "Technical literature",
                IsAvailable = true,
                AuthorId = author2.Id,
                CategoryId = category1.Id
            }
        );
        await db.SaveChangesAsync();

        return author1.Id;
    }

    [Fact]
    public async Task GetBooksWithDetails_ReturnsAllBooksWithAuthorAndCategory()
    {
        var response = await _client.GetAsync("api/v1/books/with-details");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookDetailResponse>>>(json, _options);

        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().HaveCount(3);

        var first = result.Data!.First(b => b.Title == "Clean Code");
        first.Author.Should().NotBeNull();
        first.Author.FirstName.Should().Be("Robert");
        first.Author.LastName.Should().Be("Martin");
        first.Category.Should().NotBeNull();
        first.Category.Name.Should().Be("Technical literature");
    }

    [Fact]
    public async Task GetBooksByAuthor_ReturnsBooksBelongingToAuthor()
    {
        var response = await _client.GetAsync($"api/v1/authors/{_robertMartinAuthorId}/books");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookDetailResponse>>>(json, _options);

        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().HaveCount(2);
        result.Data.Should().AllSatisfy(b => b.Author.FirstName.Should().Be("Robert"));
    }

    [Fact]
    public async Task GetCategoryStatistics_ReturnsStatistics()
    {
        var response = await _client.GetAsync("api/v1/categories/statistics");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<CategoryStatisticsResponse>>>(json, _options);

        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();

        var techCategory = result.Data!.First(c => c.CategoryName == "Technical literature");
        techCategory.BookCount.Should().Be(3);
        techCategory.AveragePublicationYear.Should().BeGreaterThan(0);
    }
}
