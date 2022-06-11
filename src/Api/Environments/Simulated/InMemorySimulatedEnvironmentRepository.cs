namespace Paladin.Api.Environments.Simulated;

public class InMemorySimulatedEnvironmentRepository : ISimulatedEnvironmentRepository
{
    private readonly Dictionary<EnvironmentId, SimulatedEnvironment> _environments;

    public InMemorySimulatedEnvironmentRepository()
    {
        _environments = new Dictionary<EnvironmentId, SimulatedEnvironment>();
    }

    public async IAsyncEnumerable<SimulatedEnvironment> GetAsync()
    {
        foreach (var environment in _environments)
        {
            yield return environment.Value;
        }
    }

    public Task<SimulatedEnvironment?> GetOrDefaultAsync(EnvironmentId id)
    {
        if (_environments.TryGetValue(id, out var simulatedEnvironment))
        {
            return Task.FromResult<SimulatedEnvironment?>(simulatedEnvironment);
        }

        return Task.FromResult<SimulatedEnvironment?>(null);
    }

    public Task SaveAsync(SimulatedEnvironment environment)
    {
        _environments.TryAdd(environment.Id, environment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(EnvironmentId id)
    {
        _environments.Remove(id);

        return Task.CompletedTask;
    }

    public Task<EnvironmentId> NextIdentityAsync()
    {
        return Task.FromResult(new EnvironmentId(Guid.NewGuid().ToString()));
    }
}
