using Acti.Application.Dtos;
using Acti.Application.Interfaces;
using Acti.Core.Exceptions;
using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Acti.Application.Services;

public class UserServices : IUserServices
{
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UserServices(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDTO> Create(UserDTO userDTO)
    {
        var userExists = await _userRepository.GetByEmail(userDTO.Email);

        if (userExists != null)
            throw new ApiException("The email provided is already registered with an existing user.", 409);
        var user = _mapper.Map<User>(userDTO);

        user.Password = _passwordHasher.HashPassword(user, userDTO.Password);

        var userCreated = await _userRepository.Add(user);

        return _mapper.Map<UserDTO>(userCreated);
    }

    public async Task Delete(int id)
    {
        await _userRepository.Delete(id);
    }

    public async Task<UserDTO> FindByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> Get(int id)
    {
        var user = await _userRepository.Get(id);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<List<UserDTO>> Get()
    {
        var allUsers = await _userRepository.Get();

        return _mapper.Map<List<UserDTO>>(allUsers);
    }

    public async Task<UserDTO> Update(UserDTO userDTO)
    {
        var userExists = await _userRepository.Get(userDTO.Id);

        if (userExists != null)
        {

            var user = _mapper.Map<User>(userDTO);
            var userCreated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userCreated);
        }
        else
        {
            throw new ApiException("Nenhum usuario encontrado.", 409);
        }
    }
}