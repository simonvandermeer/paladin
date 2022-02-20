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

    public IAsyncEnumerable<Environment> GetEnvironmentsAsync()
    {
        return _contract.GetEnvironmentsAsync();
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

    public Task PostEnvironmentActionAsync(string environmentId, EnvironmentAction action)
    {
        return _contract.PostEnvironmentActionAsync(environmentId, action);
    }
}
