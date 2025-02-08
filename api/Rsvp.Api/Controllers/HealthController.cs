namespace Rsvp.Api.Controllers;

using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[TranslateResultToActionResult]
public class HealthController : ControllerBase
{
  [HttpGet("")]
  public Result<string> Health()
  {
    return Result.Success("Healthy");
  }
}
