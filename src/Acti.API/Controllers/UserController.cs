using Acti.API.ViewModels;
using Acti.Application.Dtos;
using Acti.Application.Interfaces;
using Acti.Core.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acti.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserServices _userService;

    public UserController(IUserServices userServices, IMapper mapper)
    {
        _userService = userServices;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize]
    [Route("/api/v1/users/create")]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel createUserViewModel)
    {
        try
        {
            var userDTO = _mapper.Map<UserDTO>(createUserViewModel);
            var userCreated = await _userService.Create(userDTO);

            return Ok(new ResultViewModel
            {
                Message = "User created successfully.",
                Success = true,
                Data = userCreated
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
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}