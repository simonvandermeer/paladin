using Paladin.Api.Dto.Environments;
using Environment = Paladin.Api.Dto.Environments.Environment;

namespace Paladin.Api.Sdk;

internal interface IPaladinApiContract
{
    [Get("/api/environments")]
    public Task<IEnumerable<Environment>> GetEnvironmentsAsync();

    [Get("/api/environments/{environmentId}")]
    public Task<Environment?> GetEnvironmentAsync(string environmentId);

    [Post("/api/environments")]
    public Task<Environment> PostEnvironmentAsync([Body] NewEnvironmentOptions options);

    [Post("/api/environments/{environmentId}/action")]
    public Task PostEnvironmentActionAsync(string environmentId, [Body] EnvironmentAction action);
}
