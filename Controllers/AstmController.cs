using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Protocols;

[ApiController]
[Route("api/[controller]")]
public class AstmController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AstmController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] AstmRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");

        var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Astm, _configuration);
        var protocolClient = (protocolService as BaseProtocol).GetProtocolClient(protocolService);
        await protocolService.SendMessageAsync(protocolClient, req.Message, ct);

        return Ok(new { status = "sent" });
    }
}

public record AstmRequest(string Message);
