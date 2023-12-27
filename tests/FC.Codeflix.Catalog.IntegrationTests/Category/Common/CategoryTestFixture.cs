using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.Data.ES;
using Elasticsearch.Net;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.Common;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : BaseFixture, IDisposable
{
    public CategoryTestFixture() : base()
    {
        CreateCategoryIndex().GetAwaiter().GetResult();
    }

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public DomainEntity.Category GetValidCategory() =>
        new(Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean());

    public IList<CategoryModel> GetCategoriesModelList(int count = 10)
        => Enumerable.Range(1, count)
            .Select(_ =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                return CategoryModel.FromCategory(GetValidCategory());
            })
            .ToList();

    private async Task CreateCategoryIndex()
    {
        var elasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
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
                                .Name(category => category.Name.Suffix("keywork")))
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
        var elasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        elasticClient.DeleteByQuery<CategoryModel>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed)
        );
    }

    public void Dispose()
    {
        var elasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}
