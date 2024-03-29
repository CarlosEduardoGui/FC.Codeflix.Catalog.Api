﻿using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SearchCategory;

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }

public class SearchCategoryTestFixture : CategoryTestFixture
{
    public IList<CategoryModel> GetCategoriesModelList(IEnumerable<string> categoriesName)
        => DataGenerator.GetCategoriesModelList(categoriesName);

    public IList<CategoryModel> CloneCategoryListOrdered(
        IList<CategoryModel> examples,
        string orderBy,
        SearchOrder order
    ) => DataGenerator.CloneCategoryListOrdered(examples, orderBy, order);
}
