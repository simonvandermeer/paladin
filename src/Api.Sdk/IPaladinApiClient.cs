using Paladin.Api.Dto.Environments;
using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Sdk;

public interface IPaladinApiClient
{
    public IAsyncEnumerable<Environment> GetEnvironmentsAsync();
    public Task<Environment?> GetEnvironmentAsync(string environmentId);
    public Task PostEnvironmentActionAsync(string environmentId, EnvironmentAction action);
}
