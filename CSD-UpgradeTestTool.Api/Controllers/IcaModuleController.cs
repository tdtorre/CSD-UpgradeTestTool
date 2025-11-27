using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IcaModuleController : ControllerBase
{
    // private readonly IModuleExecutionServiceOld _moduleExecutionService;

    // public IcaModuleController(IModuleExecutionServiceOld moduleExecutionService)
    // {
    //     _moduleExecutionService = moduleExecutionService;
    // }

    // [HttpGet("module-execution")]
    // public async Task<IActionResult> ModuleExecution()
    // {
    //     var project = _moduleExecutionService.InitializeProject();
    //     var executionReport = await _moduleExecutionService.IcaModuleRun(project);
    //     return Ok(new { ExecutionReport = executionReport });
    // }
}