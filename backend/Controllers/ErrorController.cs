using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
  [ApiController]
  [Route("")]
  public class ErrorController : ControllerBase
  {
    [HttpGet("/error")]
    public IActionResult HandleError() => Problem();

    [HttpGet("/error-development")]
    public IActionResult HandleErrorDevelopment(
    [FromServices] IHostEnvironment hostEnvironment)
    {
      if (!hostEnvironment.IsDevelopment())
      {
        return NotFound();
      }

      var exceptionHandlerFeature =
          HttpContext.Features.Get<IExceptionHandlerFeature>()!;

      return Problem(
          detail: exceptionHandlerFeature.Error.StackTrace,
          title: "Khang " + exceptionHandlerFeature.Error.Message);
    }
  }
}