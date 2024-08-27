using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MossadAPI.Models;
namespace MossadApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private string GenerateToken(string userIP)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = "1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678"; //TODO: remove this from code
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                new Claim(ClaimTypes.Name, userIP),
                    }
                ),
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user.Id == "SimulationServer" || user.Id == "MVC")
            {
                string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                return StatusCode(200
                    , new { token = GenerateToken(userIP) }
                    );
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new { error = "invalid credentials" });
        }
    }
}

