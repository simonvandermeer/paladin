using Microsoft.AspNetCore.Mvc;
using Paladin.Api.Dto.Environments;
using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Environments;

[ApiController]
[Route("api/[controller]")]
public class EnvironmentsController : ControllerBase
{
    private readonly EnvironmentService _environmentService;

    public EnvironmentsController(EnvironmentService environmentService)
    {
        _environmentService = environmentService;
    }

    [HttpPost]
    public async Task<ActionResult<Environment>> PostAsync(NewEnvironment newEnvironment)
    {
        var environment = await _environmentService.StartNewEnvironmentAsync(newEnvironment);

        if (environment == null)
        {
            return BadRequest("A physical environment is already running. Only one non-simulated environment can run simultanously.");
        }

        return environment.ToDto();
    }

    [HttpGet]
    public async IAsyncEnumerable<Environment> GetAsync()
    {
        var environments = _environmentService.GetAsync();

        await foreach (var environment in environments)
        {
            yield return environment.ToDto();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Environment?>> GetAsync([FromRoute] string id)
    {
        var environment = await _environmentService.GetOrDefaultAsync(id);

        if (environment == null)
        {
            return NotFound(null);
        }

        return Ok(environment.ToDto());
    }

    [HttpPost("{id}/action")]
    public async Task<ActionResult> PostEnvironmentActionAsync([FromRoute] string id, EnvironmentAction2 action)
    {
        var environment = await _environmentService.GetOrDefaultAsync(id);

        if (environment == null)
        {
            return NotFound();
        }

        environment.RunAction(new EnvironmentAction(Enum.Parse<ActionType>(action.Type, true)));

        return NoContent();
    }

    [HttpPost("{id}/reset")]
    public async Task<ActionResult> PostResetAsync([FromRoute] string id)
    {
        var environment = await _environmentService.GetOrDefaultAsync(id);

        if (environment == null)
        {
            return NotFound();
        }

        environment.Reset();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] string id)
    {
        await _environmentService.DeleteAsync(id);

        return NoContent();
    }
}
