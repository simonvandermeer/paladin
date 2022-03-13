namespace Paladin.Api.Dto.Environments;

public record NewEnvironmentOptions
{
    /// <summary>
    /// Whether or not to create a simulated or real environment.
    /// </summary>
    public bool Simulated { get; init; }
}
