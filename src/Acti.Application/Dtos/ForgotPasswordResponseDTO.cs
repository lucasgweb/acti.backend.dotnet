namespace Acti.Application.Dtos;

public class ForgotPasswordResponseDTO
{
    public ForgotPasswordResponseDTO(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}