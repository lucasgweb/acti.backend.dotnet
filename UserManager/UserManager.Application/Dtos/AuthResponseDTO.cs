using Acti.Domain.Entities;

namespace Acti.Application.Dtos;

public class AuthenticateResponseDTO
{
    public AuthenticateResponseDTO(User user, string token)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Token = token;
    }

    public AuthenticateResponseDTO()
    {
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}