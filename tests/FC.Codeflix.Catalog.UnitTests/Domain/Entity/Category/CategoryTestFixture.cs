using Bogus;
using FC.Codeflix.Catalog.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;


namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base() { }

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
}
