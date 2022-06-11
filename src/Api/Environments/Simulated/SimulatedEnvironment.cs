using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments.Simulated;

public class SimulatedEnvironment : IEnvironment
{
    public SimulatedEnvironment(SimulatedEnvironmentCreationOptions options)
    {
        Id = options.Id;
    }

    public EnvironmentId Id { get; }

    public bool Simulated => true;

    public EnvironmentState State => new EnvironmentState(new Map { Width = 1, Height = 1, Tiles = new List<List<Tile>> { new List<Tile>() } });

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
