// Shared/Results/Result.cs
namespace Shared.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public string Error { get; }
    public int StatusCode { get; }

    protected Result(T data, bool isSuccess, string error, int statusCode)
    {
        Data = data;
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data) => new Result<T>(data, true, string.Empty, 200);
    public static Result<T> Failure(string error, int statusCode = 400) => new Result<T>(default(T), false, error, statusCode);
}

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public int StatusCode { get; }

    protected Result(bool isSuccess, string error, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Success() => new Result(true, string.Empty, 200);
    public static Result Failure(string error, int statusCode = 400) => new Result(false, error, statusCode);
}
