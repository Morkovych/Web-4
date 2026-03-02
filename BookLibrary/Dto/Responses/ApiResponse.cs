namespace BookLibrary.Dto.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public string Message { get; set; } = "Operation completed successfully.";
}

public class ApiResponse
{
    public bool Success { get; set; } = false;
    public object? Data { get; set; }
    public string Message { get; set; } = "An error occurred.";
}