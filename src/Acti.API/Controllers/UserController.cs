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
    [Route("/api/v1/users/")]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel createUserViewModel)
    {
        try
        {
            var userDTO = _mapper.Map<UserDTO>(createUserViewModel);
            var userCreated = await _userService.Create(userDTO);

            return Ok(new ResultViewModel
            {
                Message = "Usuario criado com sucesso.",
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

    [HttpGet]
    [Authorize]
    [Route("api/v1/users")]
    public async Task<IActionResult> Get([FromQuery] string email = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userService.FindByEmail(email);

                return Ok(new ResultViewModel
                {
                    Message = "Success",
                    Success = true,
                    Data = user
                });
            }
            else
            {
                var users = await _userService.Get();

                return Ok(new ResultViewModel
                {
                    Message = "Success",
                    Success = true,
                    Data = users
                });
            }
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

    [HttpGet]
    [Authorize]
    [Route("api/v1/users/{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var user = await _userService.Get(id);

            return Ok(new ResultViewModel
            {
                Message = "Success",
                Success = true,
                Data = user
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

    [HttpPut]
    [Authorize]
    [Route("api/v1/users")]
    public async Task<IActionResult> Update([FromBody] UpdateUserViewModel updateUserViewModel)
    {
        try
        {
            var userDTO = _mapper.Map<UserDTO>(updateUserViewModel);
            var userUpdated = await _userService.Update(userDTO);

            return Ok(new ResultViewModel
            {
                Message = "Usuario atualizado com sucesso.",
                Success = true,
                Data = userUpdated
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

    [HttpDelete]
    [Authorize]
    [Route("api/v1/users/{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _userService.Delete(id);

            return Ok(new ResultViewModel
            {
                Message = "Usuario excluido com sucesso.",
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
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}