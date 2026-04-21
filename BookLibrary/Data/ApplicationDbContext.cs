using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);

            entity.HasData(
                new Author { Id = 1, FirstName = "Robert", LastName = "Martin", Bio = "Software engineer and author" },
                new Author { Id = 2, FirstName = "George", LastName = "Orwell", Bio = "British writer, journalist and literary critic, radio host, memoirist, publicist" }
            );
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(150);
            entity.HasIndex(c => c.Name).IsUnique();

            entity.HasData(
                new Category { Id = 1, Name = "Technical literature", Description = "Books about software development" },
                new Category { Id = 2, Name = "Science Fiction", Description = "Sci-fi books" }
            );
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(250);
            entity.Property(b => b.Isbn).HasMaxLength(20);
            entity.HasIndex(b => b.Isbn).IsUnique();
            entity.Property(b => b.Genre).HasMaxLength(150);

            entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Book { Id = 1, Title = "Clean Code", AuthorId = 1, CategoryId = 1, Isbn = "978-11-1111-000-1", PublicationYear = 2008, Genre = "Technical literature", IsAvailable = true },
                new Book { Id = 2, Title = "Clean Architecture", AuthorId = 1, CategoryId = 1, Isbn = "978-11-1111-000-2", PublicationYear = 2017, Genre = "Technical literature", IsAvailable = true },
                new Book { Id = 3, Title = "Clean Agile", AuthorId = 1, CategoryId = 1, Isbn = "978-11-1111-000-3", PublicationYear = 2019, Genre = "Technical literature", IsAvailable = true },
                new Book { Id = 4, Title = "1984", AuthorId = 2, CategoryId = 2, Isbn = "978-11-1111-000-4", PublicationYear = 1949, Genre = "Science Fiction", IsAvailable = true }
            );
        });
    }
}