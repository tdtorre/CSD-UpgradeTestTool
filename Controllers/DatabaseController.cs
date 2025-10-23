using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public DatabaseController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] QueryRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Query)) return BadRequest("Query is required");
        var res = await _databaseService.ExecuteQueryAsync(req.Query);
        return Ok(new { result = res });
    }

    [HttpGet("test-mapping")]
    public IActionResult GetTestMapping()
    {
        var res = _databaseService.GetTestMappings();
        return Ok(new { result = res });
    }
}

public record QueryRequest(string Query);
