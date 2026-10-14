using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Create(RegisterRequest request)
    {
        var user = await _service.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, user);
    }


    
}
