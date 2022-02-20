namespace Paladin.Api.Environments;

public interface ISimulatedEnvironmentsRepository
{
    Task SaveAsync(SimulatedEnvironment environment);
    IAsyncEnumerable<SimulatedEnvironment> GetAsync();
    Task<SimulatedEnvironment> GetOrDefaultAsync(string id);
    Task DeleteAsync(string id);
}
