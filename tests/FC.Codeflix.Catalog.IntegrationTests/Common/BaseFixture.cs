using Bogus;
using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Common;
public class BaseFixture
{
    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
        ServiceProvider = BuildServiceProvider();
    }

    public Faker Faker { get; set; }

    public IServiceProvider ServiceProvider { get; }

    public static bool GetRandomBoolean() 
        => new Random().NextDouble() <= 0.5;

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
        {
            { "ConnectionStrings:ElasticSearch", "http://localhost:9200" }
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
