using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("api/[controller]")]
public class IcaModuleController : ControllerBase
{
    private readonly IModuleExecutionService _moduleExecutionService;

    public IcaModuleController(IModuleExecutionService moduleExecutionService)
    {
        _moduleExecutionService = moduleExecutionService;
    }

    [HttpGet("module-execution")]
    public async Task<IActionResult> ModuleExecution()
    {
        var project = _moduleExecutionService.InitializeProject();
        var executionReport = await _moduleExecutionService.IcaModuleRun(project);
        return Ok(new { ExecutionReport = executionReport });
    }
}