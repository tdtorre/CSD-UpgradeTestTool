using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("api/[controller]")]
public class AstmController : ControllerBase
{
    private readonly IAstmService _astm;

    public AstmController(IAstmService astm)
    {
        _astm = astm;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] AstmRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Host) || req.Port <= 0) return BadRequest("Host and port are required");
        if (string.IsNullOrWhiteSpace(req.Message)) return BadRequest("Message is required");
        await _astm.SendMessageAsync(req.Host, req.Port, req.Message, ct);
        return Ok(new { status = "sent" });
    }
}

public record AstmRequest(string Host, int Port, string Message);
