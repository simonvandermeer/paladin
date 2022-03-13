using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments;

public class EnvironmentService
{
    private readonly ISimulatedEnvironmentsRepository _simulatedEnvironmentsRepository;

    private readonly object _physicalEnvironmentLock = new object();
    private PhysicalEnvironment? _physicalEnvironment;

    public EnvironmentService(ISimulatedEnvironmentsRepository simulatedEnvironmentsRepository)
    {
        _simulatedEnvironmentsRepository = simulatedEnvironmentsRepository;
    }

    public async Task<IEnvironment?> StartNewEnvironmentAsync(NewEnvironmentOptions options)
    {
        if (options.Simulated)
        {
            var simulatedEnvironment = new SimulatedEnvironment();
            await _simulatedEnvironmentsRepository.SaveAsync(simulatedEnvironment);

            return simulatedEnvironment;
        }

        // Physical environment. Can only be one.
        lock (_physicalEnvironmentLock)
        {
            if (_physicalEnvironment != null)
            {
                return default;
            }

            _physicalEnvironment = PhysicalEnvironment.Create();
            //_physicalEnvironment.RunAction(new Dto.Environments.Action { Type = Dto.Environments.ActionType.MoveRight });
        }

        return _physicalEnvironment;
    }

    public async IAsyncEnumerable<IEnvironment> GetAsync()
    {
        if (_physicalEnvironment != null)
        {
            yield return _physicalEnvironment;
        }

        var simulatedEnvironments = _simulatedEnvironmentsRepository.GetAsync();

        await foreach (var simulatedEnvironment in simulatedEnvironments)
        {
            yield return simulatedEnvironment;
        }
    }

    public async Task<IEnvironment?> GetOrDefaultAsync(string id)
    {
        var simulatedEnvironment = await _simulatedEnvironmentsRepository.GetOrDefaultAsync(id);

        if (simulatedEnvironment != null)
        {
            return simulatedEnvironment;
        }

        lock (_physicalEnvironmentLock)
        {
            if (_physicalEnvironment != null && _physicalEnvironment.Id == id)
            {
                if (!_physicalEnvironment.IsUp)
                {
                    _physicalEnvironment = null;
                    return default;
                }

                return _physicalEnvironment;
            }
        }

        return default;
    }

    public async Task DeleteAsync(string id)
    {
        var simulatedEnvironment = await _simulatedEnvironmentsRepository.GetOrDefaultAsync(id);

        if (simulatedEnvironment != null)
        {
            await _simulatedEnvironmentsRepository.DeleteAsync(id);

            return;
        }

        lock (_physicalEnvironmentLock)
        {
            if (_physicalEnvironment != null && _physicalEnvironment.Id == id)
            {
                _physicalEnvironment.Close();
                _physicalEnvironment = null;
            }
        }
    }
}
