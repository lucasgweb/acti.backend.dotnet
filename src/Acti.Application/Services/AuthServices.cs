using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Acti.Application.Dtos;
using Acti.Application.Interfaces;
using Acti.Core.Exceptions;
using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using Acti.Infra.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Acti.Application.Services;

public class AuthServices : IAuthServices
{
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;


    public AuthServices(IEmailSender emailSender, IUserRepository userRepository, IConfiguration configuration)
    {
        _emailSender = emailSender;
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthenticateResponseDTO> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null) throw new ApiException("Invalid login or password.");

        var passwordHasher = new PasswordHasher<User>();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (passwordVerification == PasswordVerificationResult.Failed)
            throw new ApiException("Invalid login or password.");

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

    public async Task<ForgotPasswordResponseDTO> ForgotPassword(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user == null) throw new ApiException("User does not exist");

        var token = Guid.NewGuid().ToString();


        user.ResetToken = token;

        await _userRepository.Update(user);
        var appSettings = _configuration.GetSection("AppSettings");
        var resetUrl = $"{appSettings["ApiUrl"]}/v1/auth/reset-password/{token}?email={user.Email}";

        var emailBody = $@"<html>
            <head>
              <title>Notificação de redefinição de senha</title>
            </head>
            <body>
              <h2>Redefinição de senha solicitada</h2>
              <p>Foi solicitada uma redefinição de senha para sua conta. Clique no link abaixo para continuar:</p>
              <p><a href='{resetUrl}\'>{resetUrl}</a></p>
                < p > Se você não solicitou a redefinição de senha, ignore esta mensagem.</ p >
                < p > Obrigado,</ p >
                < p > Equipe de suporte</ p >
                </ body >
                </ html > ";

        await _emailSender.SendEmailAsync(user.Email, "Notificación de restablecimiento de contraseña", emailBody);

        var response =
            new ForgotPasswordResponseDTO(
                "Le hemos enviado un correo electrónico con un enlace para restablecer su contraseña.");

        return response;
    }

    public async Task ResetPassword(string email, string token, string password)
    {
        var user = await _userRepository.GetByEmail(email) ?? throw new ApiException("User does not exist");

        if (user.ResetToken != null && user.ResetToken != token) throw new ApiException("Token inválido.", 401);

        user.Password = _passwordHasher.HashPassword(user, password);
        user.ResetToken = "";

        await _userRepository.Update(user);
    }
}