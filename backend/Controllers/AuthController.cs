using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnOAuth.Models;
using LearnOAuth.Models.DTOs;
using LearnOAuth.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnOAuth.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly UserOAuthServices userOAuthServices;
    private readonly JwtServices jwtServices;

    public AuthController(UserOAuthServices _userOAuthServices, JwtServices _jwtServices)
    {
      userOAuthServices = _userOAuthServices;
      jwtServices = _jwtServices;
    }

    [HttpPost("register")]
    public async Task<ActionResult> PostCreateUser([FromBody] User userData)
    {
      string username = userData.username;
      var userExisted = await userOAuthServices.GetUserByUsername(username);
      if (userExisted == null)
      {
        string passwordHashed = BCrypt.Net.BCrypt.HashPassword(userData.password);
        string Role = userData.Role;
        User newUser = new User
        {
          username = username,
          password = passwordHashed,
          Role = Role
        };
        await userOAuthServices.CreateNewUserAsync(newUser);
        return Ok(await userOAuthServices.GetUserByUsername(username));
      }
      else
      {
        return StatusCode(400, "User existed");
      }
    }

    [HttpPost("login")]
    public async Task<ActionResult> PostLogin([FromBody] UserDTO userData)
    {
      string username = userData.username;
      string password = userData.password.Trim();
      User userExisted = await userOAuthServices.GetUserByUsername(username);

      if (userExisted == null)
      {
        return StatusCode(400, new { message = "User invalid" });
      }
      else
      {
        string passwordHashed = userExisted.password;
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, passwordHashed);
        if (isValidPassword)
        {
          var dataUser = await userOAuthServices.GetUserByUsername(userData.username);
          var responseData = new
          {
            username = dataUser.username,
            Role = dataUser.Role,
            accessToken = jwtServices.CreateAccessToken(dataUser),
            refreshToken = jwtServices.CreateRefreshToken(dataUser)
          };
          return Ok(new { message = "Login succesfully", data = responseData });
        }
        else
        {
          return StatusCode(401, new { message = "Invalid username or password" });
        }
      }
    }

  }
}