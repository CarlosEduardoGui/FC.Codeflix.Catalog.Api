using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseTestFixtureCollection : ICollectionFixture<CategoryUseCaseFixture> { }

public class CategoryUseCaseFixture
{
    public CategoryDataGenerator DataGenerator { get; }

    public CategoryUseCaseFixture()
        => DataGenerator = new CategoryDataGenerator();

    public static ICategoryRepository GetMockRepository()
        => Substitute.For<ICategoryRepository>();

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();
}
