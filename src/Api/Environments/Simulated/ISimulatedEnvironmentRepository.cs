namespace Paladin.Api.Environments.Simulated;

public interface ISimulatedEnvironmentRepository
{
    Task<EnvironmentId> NextIdentityAsync();
    Task SaveAsync(SimulatedEnvironment environment);
    IAsyncEnumerable<SimulatedEnvironment> GetAsync();
    Task<SimulatedEnvironment?> GetOrDefaultAsync(EnvironmentId id);
    Task DeleteAsync(EnvironmentId id);
}
