using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.DTO;
using SurveySystem.API.Services.InterfaceServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new user"
    )]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await userService.CreateUserAsync(request.Username, request.Email, request.Password,
                request.FullName);
            return Ok(new { user.Id, user.UserName, user.FullName });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("getById{id}")]
    [SwaggerOperation(
        Summary = "Get a user by ID",
        Description = "Retrieves the details of a user by its unique identifier."
    )]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new { user.Id, user.UserName, user.FullName });
    }

    [HttpGet]
    [Route("all")]
    [SwaggerOperation(
        Summary = "Get information about all users",
        Description = "Get all users"
    )]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users.Select(u => new { u.Id, u.UserName, u.FullName }));
    }
}