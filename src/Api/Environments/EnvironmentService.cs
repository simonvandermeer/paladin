using Paladin.Api.Dto.Environments;
using Paladin.Api.Environments.Physical;
using Paladin.Api.Environments.Simulated;

namespace Paladin.Api.Environments;

public class EnvironmentService
{
    private readonly ISimulatedEnvironmentRepository _simulatedEnvironmentsRepository;

    private readonly object _physicalEnvironmentLock = new object();
    private PhysicalEnvironment? _physicalEnvironment;

    public EnvironmentService(ISimulatedEnvironmentRepository simulatedEnvironmentsRepository)
    {
        _simulatedEnvironmentsRepository = simulatedEnvironmentsRepository;
    }

    public async Task<IEnvironment?> StartNewEnvironmentAsync(NewEnvironmentOptions options)
    {
        if (options.Simulated)
        {
            var identity = await _simulatedEnvironmentsRepository.NextIdentityAsync();
            var simulatedEnvironmentOptions = new SimulatedEnvironmentCreationOptions(identity, false);

            var simulatedEnvironment = new SimulatedEnvironment(simulatedEnvironmentOptions);
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

    public async Task<IEnvironment?> GetOrDefaultAsync(EnvironmentId id)
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

    public async Task DeleteAsync(EnvironmentId id)
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
