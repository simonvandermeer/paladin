using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments.Simulated;

public class SimulatedEnvironment : IEnvironment
{
    public SimulatedEnvironment(SimulatedEnvironmentCreationOptions options)
    {
        if (options.EnableFogOfWar)
        {
            throw new NotSupportedException("Fog of war is not yet supported.");
        }

        Id = options.Id;
        Map = options.Map;
    }

    public EnvironmentId Id { get; }
    public CotNd.Map Map { get; }

    public bool Simulated => true;

    public EnvironmentState GetState()
    {
        return new EnvironmentState(Map);
    }

    public void RunAction(EnvironmentAction action)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}
