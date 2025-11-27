using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController]
[Route("config-test")]
public class ConfigController : ControllerBase
{
    // private readonly GlobalSettings _settings;

    // public ConfigController(IOptions<GlobalSettings> opts)
    // {
    //     _settings = opts.Value;
    // }

    // [HttpGet]
    // public IActionResult Get() => Ok(_settings);
}