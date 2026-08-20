using Microsoft.AspNetCore.Mvc;
using RestFul.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService ts) => _tokenService = ts;

    [HttpPost("login")]
    public IActionResult Login(LoginRequest login)
    {
        if (login.Usuario == "admin" && login.Senha == "123456")
        {
            var token = _tokenService.GerarToken(login.Usuario, "Admin");
            return Ok(new { token });
        }

        if (login.Usuario == "user" && login.Senha == "123456")
        {
            var token = _tokenService.GerarToken(login.Usuario, "User");
            return Ok(new { token });
        }

        return Unauthorized();
    }
}