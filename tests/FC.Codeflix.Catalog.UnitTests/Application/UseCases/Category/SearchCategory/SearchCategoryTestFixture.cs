using FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SearchCategory;

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }

public class SearchCategoryTestFixture : CategoryUseCaseFixture
{
    public SearchCategoryInput GetSearchInput()
    {
        var random = new Random();
        return new SearchCategoryInput(
            random.Next(1, 10),
            random.Next(10, 20),
            DataGenerator.Faker.Commerce.ProductName(),
            DataGenerator.Faker.Commerce.ProductName(),
            random.Next(0, 2) == 0 ? SearchOrder.ASC : SearchOrder.DESC
        );
    }

    public List<DomainEntity.Category> GetCategoriesList(int lenght = 10)
        => Enumerable
            .Range(0, lenght)
            .Select(_ => GetValidCategory())
            .ToList();
}
