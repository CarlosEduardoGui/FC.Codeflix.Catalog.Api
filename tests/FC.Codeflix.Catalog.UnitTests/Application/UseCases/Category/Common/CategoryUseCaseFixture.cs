﻿using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseTestFixtureCollection : ICollectionFixture<CategoryUseCaseFixture> { }

public class CategoryUseCaseFixture : BaseFixture
{
    public static Mock<ICategoryRepository> GetMockRepository()
        => new();

    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public string GetDescription()
        => Faker.Commerce.ProductDescription();
}