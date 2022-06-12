using Paladin.Api.Dto.Environments;

namespace Paladin.Api.Environments;

public interface IEnvironment
{
    EnvironmentId Id { get; }
    bool Simulated { get; }

    EnvironmentState GetState();
    void RunAction(EnvironmentAction action);
    void Close();
    void Reset();
}
