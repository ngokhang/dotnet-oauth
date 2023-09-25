using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using LearnOAuth.Models;
using LearnOAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnOAuth.Controllers
{
  [ApiController]
  [Route("api/profile")]
  public class ProfileUserController : ControllerBase
  {
    private UserServices UserServices;
    private readonly JwtServices jwtServices;

    public ProfileUserController(UserServices userServices, JwtServices _jwtServices)
    {
      UserServices = userServices;
      jwtServices = _jwtServices;
    }

    [Authorize(Roles = "user, admin")]
    [HttpGet("me")]
    public async Task<ActionResult> GetProfile([FromHeader] string accessToken)
    {
      var isTokenValid = jwtServices.CheckTokenIsValid(accessToken);
      if (isTokenValid)
      {
        var username = jwtServices.GetDataFromToken(accessToken);
        var responseData = await UserServices.GetUserByUsername(username);

        return Ok(new
        {
          data = responseData
        });
      }
      return StatusCode(401, new { ErrorCode = 401, Message = "Token has expired" });
    }
  }
}