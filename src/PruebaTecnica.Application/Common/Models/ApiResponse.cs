using System.Net;
using System.Text.Json.Serialization;

namespace PruebaTecnica.Application.Common.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; init; }

    public int StatusCode { get; init; }

    public static ApiResponse<T> Success(T? data, int statusCode =(int)HttpStatusCode.OK)
    {
        return new()
        {
            IsSuccess = true,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> Failure(string error, int statusCode)
    {
        return new()
        {
            IsSuccess = false,
            Error = error,
            StatusCode = statusCode
        };
    }

}
