using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MossadAPI.MiddleWares.Global
{
    public class JwtAuth
    {
        private readonly RequestDelegate _next;

        public JwtAuth(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string bearerToken = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            string token = bearerToken.Split(" ").Last();
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678");
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await httpContext.Response.WriteAsync("Token has expired");
                        return;
                    }
                }
                catch
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Invalid Token");
                    return;
                }

            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("token is missing");
                return;
            }
            await _next(httpContext);
        }
    }
}