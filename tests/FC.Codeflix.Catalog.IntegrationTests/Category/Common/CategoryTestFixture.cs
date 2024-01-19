using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.Common;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : BaseFixture, IDisposable
{
    public CategoryDataGenerator DataGenerator { get; }
    public IElasticClient ElasticClient { get; }

    public CategoryTestFixture() : base()
    {
        ElasticClient = ServiceProvider
            .GetRequiredService<IElasticClient>();
        DataGenerator = new CategoryDataGenerator();
        ElasticSearchOperations
            .CreateCategoryIndex(ElasticClient)
            .GetAwaiter()
            .GetResult();
    }

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();

    public IList<CategoryModel> GetCategoriesModelList(int count = 10)
        => DataGenerator.GetCategoriesModelList(count);

    public void DeleteAll()
        => ElasticSearchOperations.DeleteAllCategoriesDocuments(ElasticClient);

    public void Dispose()
        => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
}
