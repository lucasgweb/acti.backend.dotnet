namespace Acti.Core.Exceptions;

public class ApiException : Exception
{
    public ApiException(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public ApiException(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
    public int StatusCode { get; set; }
}