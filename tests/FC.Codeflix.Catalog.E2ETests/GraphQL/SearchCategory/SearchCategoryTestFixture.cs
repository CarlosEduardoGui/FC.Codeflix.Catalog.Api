using RepositoryDTO = FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.SearchCategory;

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }

public class SearchCategoryTestFixture : CategoryTestFixture
{
    public IList<CategoryModel> GetCategoriesModelList(IEnumerable<string> categoriesName)
        => DataGenerator.GetCategoriesModelList(categoriesName);

    public IList<CategoryModel> CloneCategoryListOrdered(
        IList<CategoryModel> examples,
        string orderBy,
        RepositoryDTO.SearchOrder order
    )
        => DataGenerator.CloneCategoryListOrdered(examples, orderBy, order);
}
