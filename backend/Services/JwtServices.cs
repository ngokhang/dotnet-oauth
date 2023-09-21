using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LearnOAuth.Models;
using Microsoft.IdentityModel.Tokens;

namespace LearnOAuth.Services
{
  public class JwtServices
  {
    private readonly IConfiguration _config;

    public JwtServices(IConfiguration config)
    {
      _config = config;
    }

    public string CreateAccessToken(User user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var claims = new List<Claim> {
        new Claim ("name", user.username),
        new Claim ("Role", user.Role)
    };
      JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: credentials);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string CreateRefreshToken(User user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var claims = new List<Claim> {
        new Claim (ClaimTypes.Name, user.username),
        new Claim (ClaimTypes.Role, user.Role)
    };
      JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool CheckTokenIsValid(string tokenFromHeader)
    {
      var handler = new JwtSecurityTokenHandler();
      var token = handler.ReadJwtToken(tokenFromHeader);
      var expiresOn = token.ValidTo;
      var currentTime = DateTimeOffset.UtcNow;

      if (expiresOn >= currentTime)
      {
        return true; // token still valid
      }
      return false; // token has expired
    }

    public string GetDataFromToken(string tokenFromHeader)
    {
      var handler = new JwtSecurityTokenHandler();

      // Read the JWT token
      var token = handler.ReadJwtToken(tokenFromHeader);
      var username = token.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;

      return username == null ? "null" : username.ToString();
    }


  }
}