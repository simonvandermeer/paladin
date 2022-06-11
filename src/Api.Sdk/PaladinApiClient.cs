using Paladin.Api.Dto.Environments;
using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Sdk;

internal class PaladinApiClient : IPaladinApiClient
{
    private readonly IPaladinApiContract _contract;

    public PaladinApiClient(IPaladinApiContract contract)
    {
        _contract = contract ?? throw new ArgumentNullException(nameof(contract));
    }

    public async IAsyncEnumerable<Environment> GetEnvironmentsAsync()
    {
        foreach (var environment in await _contract.GetEnvironmentsAsync())
        {
            yield return environment;
        }
    }

    public async Task<Environment?> GetEnvironmentAsync(string environmentId)
    {
        try
        {
            return await _contract.GetEnvironmentAsync(environmentId);
        }
        catch (ApiException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public Task<Environment> PostEnvironmentAsync(NewEnvironmentOptions options)
    {
        return _contract.PostEnvironmentAsync(options);
    }

    public Task PostEnvironmentActionAsync(string environmentId, EnvironmentAction action)
    {
        return _contract.PostEnvironmentActionAsync(environmentId, action);
    }
}
