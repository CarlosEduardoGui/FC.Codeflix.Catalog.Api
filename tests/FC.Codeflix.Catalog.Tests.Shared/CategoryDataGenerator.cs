using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Tests.Shared;
public class CategoryDataGenerator : DataGeneratorBase
{
    public CategoryDataGenerator() : base()
    {

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
            DateTime.UtcNow.Date,
            GetRandomBoolean());

    public IList<CategoryModel> GetCategoriesModelList(int count = 10)
        => Enumerable.Range(1, count)
            .Select(_ =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                return CategoryModel.FromCategory(GetValidCategory());
            })
            .ToList();

    public IList<CategoryModel> GetCategoriesModelList(IEnumerable<string> categoriesName)
        => categoriesName.Select(name =>
        {
            Task.Delay(5).GetAwaiter().GetResult();
            var category = CategoryModel.FromCategory(GetValidCategory());
            category.Name = name;
            return category;
        }).ToList();

    public IList<CategoryModel> CloneCategoryListOrdered(
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
