using FluentValidation;
using BookLibrary.Dto.Requests;

namespace BookLibrary.Validators;

public class BookValidator : AbstractValidator<BookRequest>
{
    public BookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(1, 250).WithMessage("Title length must be between 1 and 250 characters.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.")
            .Length(2, 150).WithMessage("Author length must be between 2 and 150 characters.");

        RuleFor(x => x.Isbn)
            .Matches(@"^(978|979)-\d{2}-\d{4}-\d{3}-\d{1}$")
            .When(x => !string.IsNullOrEmpty(x.Isbn))
            .WithMessage("ISBN must follow the ISBN-13 XXX-XX-XXXX-XXX-X (start from 978|979 number).");

        RuleFor(x => x.PublicationYear)
            .GreaterThanOrEqualTo(1000).WithMessage("The year of publication cannot be less than 1000.")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage($"The year of publication cannot be greater {DateTime.Now.Year}.");

        RuleFor(x => x.Genre)
            .Length(2, 150).WithMessage("Genre length must be between 2 and 150 characters.");
    }
}