using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Acti.Application.Dtos;
using Acti.Application.Interfaces;
using Acti.Core.Exceptions;
using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Acti.Application.Services;

public class AuthServices : IAuthServices
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;


    public AuthServices(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthenticateResponseDTO> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null) throw new AppException("Invalid login or password.");

        var passwordHasher = new PasswordHasher<User>();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (passwordVerification == PasswordVerificationResult.Failed)
            throw new AppException("Invalid login or password.");

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        if (string.IsNullOrEmpty(secretKey)) throw new ArgumentException("The secret key cannot be null or empty.");

        var key = Encoding.ASCII.GetBytes(secretKey);

        var expires = DateTime.UtcNow.AddDays(int.Parse(jwtSettings["HoursToExpire"]));


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            }),
            Expires = expires,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var response = new AuthenticateResponseDTO
        {
            Token = tokenHandler.WriteToken(token),
            Name = user.Name,
            Email = user.Email,
            Id = user.Id
        };

        return response;
    }
}