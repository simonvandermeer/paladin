using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Environments;

internal static class IEnvironmentExtensions
{
    public static Environment ToDto(this IEnvironment environment)
    {
        return new Environment(
            environment.Id,
            environment.Simulated,
            environment.State);
    }
}
