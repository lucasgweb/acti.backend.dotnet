using Acti.API.ViewModels;
using Acti.API.ViewModels.Auth;
using Acti.Application.Interfaces;
using Acti.Core.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Acti.API.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;


    public AuthController(IConfiguration configuration, IAuthServices authServices,
        IMapper mapper)
    {
        _configuration = configuration;
        _authServices = authServices;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("/api/v1/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        try
        {
            var authenticatedUser = await _authServices.Authenticate(loginViewModel.Email, loginViewModel.Password);

            return Ok(new ResultViewModel
            {
                Message = "User authenticated successfully",
                Success = true,
                Data = authenticatedUser
            });
        }
        catch (ApiException ex)
        {
            return BadRequest(new ResultViewModel
            {
                Message = ex.Message,
                Success = false
            });
        }
    }

    [HttpPost]
    [Route("api/v1/auth/forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel forgotPassword)
    {
        try
        {
            var response = await _authServices.ForgotPassword(forgotPassword.Email);

            return Ok(new ResultViewModel
            {
                Message = "Usuario autenticado com sucesso.",
                Success = true,
                Data = response
            });
        }
        catch (ApiException ex)
        {
            return BadRequest(new ResultViewModel
            {
                Message = ex.Message,
                Success = false
            });
        }
    }

    [HttpPost]
    [Route("api/v1/auth/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPassword)
    {
        try
        {
            await _authServices.ResetPassword(resetPassword.Email, resetPassword.Token, resetPassword.Password);

            return Ok(new ResultViewModel
            {
                Message = "Senha alterada com sucesso.",
                Success = true
            });
        }
        catch (ApiException ex)
        {
            return BadRequest(new ResultViewModel
            {
                Message = ex.Message,
                Success = false
            });
        }
    }
}