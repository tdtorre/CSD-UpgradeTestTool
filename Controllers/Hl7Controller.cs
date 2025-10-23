using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Protocols;

[ApiController]
[Route("api/[controller]")]
public class Hl7Controller : ControllerBase
{

    private readonly IConfiguration _configuration;

    public Hl7Controller(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] Hl7Request req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Host) || req.Port <= 0) return BadRequest("Host and port are required");
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");
        
        var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Astm, _configuration);
        var protocolClient = (protocolService as BaseProtocol).GetProtocolClient(protocolService);
        await protocolService.SendMessageAsync(protocolClient, req.Message, ct);
        
        return Ok(new { status = "sent" });
    }
}

public record Hl7Request(string Host, int Port, string Message);
