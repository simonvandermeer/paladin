namespace Paladin.Api.Environments;

public record NewEnvironment
{
    /// <summary>
    /// Whether or not to create a simulated or real environment.
    /// </summary>
    public bool Simulated { get; init; }
}
