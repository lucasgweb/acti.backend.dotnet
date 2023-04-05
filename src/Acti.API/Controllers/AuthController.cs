using Acti.API.Utils;
using Acti.API.ViewModels;
using Acti.Application.Interfaces;
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
        catch (Exception)
        {
            return StatusCode(500, Responses.ApplicationErrorMessage());
        }
    }
}