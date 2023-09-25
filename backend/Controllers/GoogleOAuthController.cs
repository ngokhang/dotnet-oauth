using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using LearnOAuth.Models;
using LearnOAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace LearnOAuth.Controllers
{
  [ApiController]
  [Route("auth/google")]
  public class GoogleOAuthController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly HttpServices httpServices;
    private readonly UserServices UserServices;
    private readonly JwtServices jwt;
    public GoogleOAuthController(IMapper mapper, HttpServices _httpServices, UserServices _UserServices, JwtServices _jwt)
    {
      _mapper = mapper;
      httpServices = _httpServices;
      UserServices = _UserServices;
      jwt = _jwt;
    }
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CodeModel code)
    {
      var result = await httpServices.Post(code.code.ToString());
      return Ok(result);
    }

    [Authorize(Roles = "user")]
    [HttpGet("me")]
    public async Task<ActionResult> Get([FromHeader] string Authorization)
    {
      var result = await httpServices.Get(Authorization);
      var userExisted = await UserServices.GetUserByGoogleId(result.Value.sub);
      if (userExisted == null)
      {
        var newUser = new User
        {
          username = result.Value.email,
          password = "",
          Role = "user",
          googleId = result.Value.sub
        };
        await UserServices.CreateNewUserAsync(newUser);
      }
      var user = _mapper.Map<User>(result.Value);
      var responseData = new
      {
        username = result.Value.email,
        name = result.Value.name,
        access_token = jwt.CreateAccessToken(user),
        refresh_token = jwt.CreateRefreshToken(user)
      };
      return Ok(responseData);
    }



  }
}