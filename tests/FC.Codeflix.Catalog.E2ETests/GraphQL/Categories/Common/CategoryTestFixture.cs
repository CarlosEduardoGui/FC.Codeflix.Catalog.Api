using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> WebbAppFactory { get; }
    public CatalogClient GraphQLClient { get; }
    public IElasticClient ElasticClient { get; }
    public CategoryDataGenerator DataGenerator { get; }

    public CategoryTestFixture()
    {
        DataGenerator = new CategoryDataGenerator();
        WebbAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebbAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebbAppFactory.BaseURL)
        });
        ElasticClient = WebbAppFactory.Services.GetRequiredService<IElasticClient>();
        GraphQLClient = WebbAppFactory.Services.GetRequiredService<CatalogClient>();

        ElasticSearchOperations
            .CreateCategoryIndex(ElasticClient)
            .GetAwaiter()
            .GetResult();
    }


    public void DeleteElesticsearchAllDocument()
        => ElasticSearchOperations.DeleteAllCategoriesDocuments(ElasticClient);

    public void Dispose()
        => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);

    public IList<CategoryModel> GetCategoriesModelList(int count = 10)
        => DataGenerator.GetCategoriesModelList(count);
}
