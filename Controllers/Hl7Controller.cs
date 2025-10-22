using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("api/[controller]")]
public class Hl7Controller : ControllerBase
{
    private readonly IHl7Service _hl7;

    public Hl7Controller(IHl7Service hl7)
    {
        _hl7 = hl7;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] Hl7Request req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Host) || req.Port <= 0) return BadRequest("Host and port are required");
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");
        await _hl7.SendMessageAsync(req.Host, req.Port, req.Message, ct);
        return Ok(new { status = "sent" });
    }
}

public record Hl7Request(string Host, int Port, string Message);
