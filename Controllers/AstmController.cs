using System.Reflection;
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

    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] AstmRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");

        using var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Astm, _configuration);
        using var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(protocolService);
        var response = await protocolService.SendMessageAsync(protocolClient, req.Message, req.checkAck, ct);

        return Ok(new { message = response });
    }

    [HttpPost("sendMessageToHost")]
    public async Task<IActionResult> SendMessageToHost([FromBody] AstmExtendedRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.host) || req.port <= 0) return BadRequest("Host and port are required");
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");

        using var protocolService = ProtocolServiceFactory.GetProtocolService(ProtocolType.Astm, _configuration);
        using var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(req.host, req.port, protocolService);
        var response = await protocolService.SendMessageAsync(protocolClient, req.Message, req.checkAck, ct);

        return Ok(new { message = response });
    }
}

public record AstmRequest(string Message, bool checkAck = false);
public record AstmExtendedRequest(string host, int port, string Message, bool checkAck = false);
