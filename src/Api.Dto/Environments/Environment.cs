namespace Paladin.Api.Dto.Environments;

public record Environment(string Id, bool Simulated, EnvironmentState State);
