using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture
{
    public CategoryDataGenerator DataGenerator { get; }
    public CategoryTestFixture()
        => DataGenerator = new CategoryDataGenerator();

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();
}
