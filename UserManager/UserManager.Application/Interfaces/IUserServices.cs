using Acti.Application.Dtos;

namespace Acti.Application.Interfaces;

public interface IUserServices
{
    Task<UserDTO> Create(UserDTO userDTO);
    Task<UserDTO> Update(UserDTO userDTO);
    Task Delete(int id);
    Task<UserDTO> Get(int id);
    Task<List<UserDTO>> Get();
    Task<UserDTO> FindByEmail(string email);
}