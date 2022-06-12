using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Environments;

internal static class IEnvironmentExtensions
{
    public static Environment ToDto(this IEnvironment environment)
    {
        return new Environment(
            // TODO: Make work with converters.
            environment.Id.Id,
            environment.Simulated,
            environment.GetState());
    }
}
