using LinePayDemo.Api.Helpers;
using LinePayDemo.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var userId = await authService.AuthenticateAsync(request.Account, request.Password);

            return Ok(new ResponseModel
            {
                Message = "success",
                Data = userId
            });
        }
        catch (Exception e)
        {
            return Ok(new ResponseModel
            {
                Message = e.Message
            });
        }
    }
}

public class LoginRequest
{
    public string Account { get; set; }
    public string Password { get; set; }
}