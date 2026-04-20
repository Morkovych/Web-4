namespace BookLibrary.Dto.Responses;

public record BookResponse(
    int Id,
    string Title,
    int AuthorId,
    int CategoryId,
    string? Isbn,
    int PublicationYear,
    string? Genre,
    bool IsAvailable
);

public record BookDetailResponse(
    int Id,
    string Title,
    AuthorResponse Author,
    CategoryResponse Category,
    string? Isbn,
    int PublicationYear,
    string? Genre,
    bool IsAvailable
);

public record AuthorResponse(
    int Id,
    string FirstName,
    string LastName,
    string? Bio
);

public record CategoryResponse(
    int Id,
    string Name,
    string? Description
);

public record CategoryStatisticsResponse(
    string CategoryName,
    int BookCount,
    double AveragePublicationYear
);