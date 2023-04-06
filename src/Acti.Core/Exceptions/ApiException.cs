namespace Acti.Core.Exceptions;

public class ApiException : Exception
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public ApiException(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public ApiException(string message)
    {
        Message = message;
    }
}