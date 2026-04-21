using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookLibrary.Validators;

namespace BookLibrary.Models;

[Description("Модель книги")]
public class Book
{
    [Description("Уникальный идентификатор книги.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250, MinimumLength = 1, ErrorMessage = "Title length must be between 1 and 250 characters.")]
    [Description("Название книги. Обязательное поле.")]
    public string Title { get; set; } = null!;

    [Isbn]
    [Description("ISBN-13 книги (международный стандартный номер).")]
    public string? Isbn { get; set; }

    [Range(1000, 9999, ErrorMessage = "The year of publication must be between 1000 and 9999.")]
    [Description("Год публикации. Допустимый диапазон: 1000 – текущий год.")]
    public int PublicationYear { get; set; }

    [StringLength(150, MinimumLength = 2, ErrorMessage = "Genre length must be between 2 and 150 characters.")]
    [Description("Жанр книги.")]
    public string? Genre { get; set; }

    [Description("Указывает, доступна ли книга для выдачи.")]
    public bool IsAvailable { get; set; }

    [Description("Внешний ключ на автора.")]
    public int AuthorId { get; set; }

    [Description("Навигационное свойство автора.")]
    public Author Author { get; set; } = null!;

    [Description("Внешний ключ на категорию.")]
    public int CategoryId { get; set; }

    [Description("Навигационное свойство категории.")]
    public Category Category { get; set; } = null!;
}