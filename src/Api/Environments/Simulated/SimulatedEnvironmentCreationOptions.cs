namespace Paladin.Api.Environments.Simulated;

public class SimulatedEnvironmentCreationOptions
{
    public SimulatedEnvironmentCreationOptions(EnvironmentId id, bool enableFogOfWar = true)
    {
        if (enableFogOfWar)
        {
            throw new NotSupportedException("Fog of war is not yet supported.");
        }

        Id = id;
        FogOfWarEnabled = enableFogOfWar;
    }

    public EnvironmentId Id { get; }
    public bool FogOfWarEnabled { get; }
}
