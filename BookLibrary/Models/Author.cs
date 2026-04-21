using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models;

[Description("Модель автора")]
public class Author
{
    [Description("Уникальный идентификатор автора.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "FirstName length must be between 1 and 100 characters.")]
    [Description("Имя автора. Обязательное поле.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "LastName length must be between 1 and 100 characters.")]
    [Description("Фамилия автора. Обязательное поле.")]
    public string LastName { get; set; } = null!;

    [Description("Биография автора. Необязательное поле.")]
    public string? Bio { get; set; }

    public List<Book> Books { get; set; } = new();
}