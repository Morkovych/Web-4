using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models;

[Description("Модель категории")]
public class Category
{
    [Description("Уникальный идентификатор категории.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Name length must be between 1 and 150 characters.")]
    [Description("Название категории. Обязательное поле, уникальное.")]
    public string Name { get; set; } = null!;

    [Description("Описание категории. Необязательное поле.")]
    public string? Description { get; set; }

    public List<Book> Books { get; set; } = new();
}
