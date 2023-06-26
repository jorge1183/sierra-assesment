using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace sierra.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
  private readonly IConfiguration _configuration;

  public SessionController(IConfiguration configuration)
  {
    _configuration = configuration;
  }
  [HttpPost("login")]
  public IActionResult Login([FromQuery]string username, [FromQuery]string password)
  {
    var isValidUsername = _configuration["session-login:username"].Equals(username, StringComparison.OrdinalIgnoreCase);
    var isValidPassword = _configuration["session-login:password"].Equals(password, StringComparison.Ordinal);

    if (!isValidUsername || !isValidPassword)
    {
      return Forbid();
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.Now.AddMinutes(60),
        signingCredentials: credentials);

    var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    return Ok(new
    {
        AccessToken = jwt
    });
  }
}
