using FC.Codeflix.Catalog.Infra.Data.ES.Repositories;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Microsoft.Extensions.DependencyInjection;
using FC.Codeflix.Catalog.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Nest;

namespace FC.Codeflix.Catalog.Infra.Data.ES;
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ElasticSearch");
        var uri = new Uri(connectionString!);
        var connectionSettings = new ConnectionSettings(uri)
            .DefaultMappingFor<CategoryModel>(i => i
                .IndexName(ElasticsearchIndices.Category)
                .IdProperty(p => p.Id)
            )
            .PrettyJson()
            .ThrowExceptions()
            .RequestTimeout(TimeSpan.FromMinutes(2));

        var client = new ElasticClient(connectionSettings);
        services.AddSingleton<IElasticClient>(client);
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
