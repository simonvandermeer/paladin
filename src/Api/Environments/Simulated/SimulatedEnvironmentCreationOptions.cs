using Paladin.CotNd;

namespace Paladin.Api.Environments.Simulated;

public record SimulatedEnvironmentCreationOptions(EnvironmentId Id, Map Map, bool EnableFogOfWar = true);
