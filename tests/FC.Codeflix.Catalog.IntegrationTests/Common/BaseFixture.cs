using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.IntegrationTests.Common;
public class BaseFixture
{
    public IServiceProvider ServiceProvider { get; }

    protected BaseFixture()
    {
        ServiceProvider = BuildServiceProvider();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
        {
            { "ConnectionStrings:ElasticSearch", "http://localhost:9201" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        services
            .AddUseCases()
            .AddElasticSearch(configuration)
            .AddRepositories();

        return services.BuildServiceProvider();
    }
}
