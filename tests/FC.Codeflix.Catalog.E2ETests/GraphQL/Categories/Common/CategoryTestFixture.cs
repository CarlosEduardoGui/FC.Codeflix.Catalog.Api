using Elasticsearch.Net;
using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : IDisposable
{
    private const string SUFFIX_KEYWORD = "keyword";

    public CategoryTestFixture()
    {
        WebbAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebbAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebbAppFactory.BaseURL)
        });
        GraphQLClient = WebbAppFactory.Services.GetRequiredService<CatalogClient>();

        CreateCategoryIndex().GetAwaiter().GetResult();
    }

    public CustomWebApplicationFactory<Program> WebbAppFactory { get; private set; } = null;
    public CatalogClient GraphQLClient { get; private set; } = null;

    private async Task CreateCategoryIndex()
    {
        var elasticClient = WebbAppFactory.Services.GetRequiredService<IElasticClient>();
        var response = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Map<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix(SUFFIX_KEYWORD)))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
                )
            )
        );
    }

    public void DeleteElesticsearchAllDocument()
    {
        var elasticClient = WebbAppFactory.Services.GetRequiredService<IElasticClient>();

        elasticClient.DeleteByQuery<CategoryModel>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed)
        );
    }

    public void Dispose()
    {
        var elasticClient = WebbAppFactory.Services.GetRequiredService<IElasticClient>();

        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}
