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

    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] Hl7Request req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.message)) return BadRequest("Message is required");

        using var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Hl7, _configuration);
        using var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(protocolService);
        await protocolService.SendMessageAsync(protocolClient, req.message, req.checkAck, ct);

        return Ok(new { status = "sent" });
    }

    [HttpPost("sendMessageToHost")]
    public async Task<IActionResult> SendMessageToHost([FromBody] Hl7ExtendedRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.host) || req.port <= 0) return BadRequest("Host and port are required");
        if (string.IsNullOrWhiteSpace(req.message)) return BadRequest("Message is required");
        
        using var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Hl7, _configuration);
        using var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(req.host, req.port, protocolService);
        await protocolService.SendMessageAsync(protocolClient, req.message, req.checkAck, ct);
        
        return Ok(new { status = "sent" });
    }
}

public record Hl7Request(string message, bool checkAck = false);
public record Hl7ExtendedRequest(string host, int port, string message, bool checkAck = false);
