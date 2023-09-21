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
    private UserOAuthServices userOAuthServices;
    private readonly JwtServices jwtServices;

    public ProfileUserController(UserOAuthServices userServices, JwtServices _jwtServices)
    {
      userOAuthServices = userServices;
      jwtServices = _jwtServices;
    }

    [HttpGet("me")]
    public async Task<ActionResult> GetProfile([FromHeader] string accessToken)
    {
      var isTokenValid = jwtServices.CheckTokenIsValid(accessToken);
      if (isTokenValid)
      {
        var username = jwtServices.GetDataFromToken(accessToken);
        var responseData = await userOAuthServices.GetUserByUsername(username);

        return Ok(new
        {
          data = responseData
        });
      }
      return StatusCode(401, new { ErrorCode = 401, Message = "Token has expired" });
    }
  }
}