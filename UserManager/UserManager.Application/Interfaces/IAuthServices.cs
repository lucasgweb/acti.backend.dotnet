using Acti.Application.Dtos;

namespace Acti.Application.Interfaces;

public interface IAuthServices
{
    Task<AuthenticateResponseDTO> Authenticate(string email, string password);
}