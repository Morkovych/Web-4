namespace BookLibrary.Dto.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public string Message { get; set; } = "Operation completed successfully.";
}

public class ApiResponse : ApiResponse<object?>
{
    public ApiResponse()
    {
        Success = false;
        Message = "An error occurred.";
    }
}