namespace Paladin.Api.Environments
{
    public class InMemorySimulatedEnvironmentsRepository : ISimulatedEnvironmentsRepository
    {
        private readonly Dictionary<string, SimulatedEnvironment> _environments;

        public InMemorySimulatedEnvironmentsRepository()
        {
            _environments = new Dictionary<string, SimulatedEnvironment>();
        }

        public async IAsyncEnumerable<SimulatedEnvironment> GetAsync()
        {
            foreach (var environment in _environments)
            {
                yield return environment.Value;
            }
        }

        public Task<SimulatedEnvironment> GetOrDefaultAsync(string id)
        {
            if (_environments.TryGetValue(id, out var simulatedEnvironment))
            {
                return Task.FromResult(simulatedEnvironment);
            }

            return Task.FromResult((SimulatedEnvironment)null);
        }

        public Task SaveAsync(SimulatedEnvironment environment)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            _environments.Remove(id);

            return Task.CompletedTask;
        }
    }
}
