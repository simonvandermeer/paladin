using Microsoft.Extensions.DependencyInjection;

namespace Paladin.Api.Sdk
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPaladinApiClient(this IServiceCollection services)
        {
            var refitSettings = new RefitSettings();
            refitSettings.ContentSerializer = new SystemTextJsonContentSerializer();

            services.AddRefitClient<IPaladinApiContract>(refitSettings)
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("http://localhost:5000");
                });

            services.AddSingleton<IPaladinApiClient, PaladinApiClient>();

            return services;
        }
    }
}
