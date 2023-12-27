using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SearchCategory;

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }

public class SearchCategoryTestFixture : CategoryTestFixture
{
    public IList<CategoryModel> GetCategoriesModelList(IEnumerable<string> categoriesName)
        => categoriesName.Select(name =>
        {
            var category = CategoryModel.FromCategory(GetValidCategory());
            category.Name = name;
            return category;
        }).ToList();

    public static IList<CategoryModel> CloneCategoryListOrdered(
        IList<CategoryModel> examples, 
        string orderBy, 
        SearchOrder order
    )
    {
        var listClone = new List<CategoryModel>(examples);
        var orderEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.ASC) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            ("name", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            ("id", SearchOrder.ASC) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.ASC) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.DESC) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };

        return orderEnumerable.ToList();
    }
}
