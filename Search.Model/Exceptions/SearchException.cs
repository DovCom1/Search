namespace Search.Contract.Exceptions;

public class SearchException(string error, int statusCode = 400) : Exception(error)
{
    public int StatusCode { get; } = statusCode;
}