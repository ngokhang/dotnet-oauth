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
    private readonly UserServices UserServices;
    private readonly JwtServices jwtServices;

    public AuthController(UserServices _UserServices, JwtServices _jwtServices)
    {
      UserServices = _UserServices;
      jwtServices = _jwtServices;
    }

    [HttpPost("register")]
    public async Task<ActionResult> PostCreateUser([FromBody] User userData)
    {
      string username = userData.username;
      var userExisted = await UserServices.GetUserByUsername(username);
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
        await UserServices.CreateNewUserAsync(newUser);
        return Ok(await UserServices.GetUserByUsername(username));
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
      User userExisted = await UserServices.GetUserByUsername(username);

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
          var dataUser = await UserServices.GetUserByUsername(userData.username);
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