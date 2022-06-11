namespace Paladin.Api.Environments;

public record EnvironmentId(string Id)
{
    public static EnvironmentId Physical => new EnvironmentId("Physical");
}
