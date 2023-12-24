using FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SaveCategory;

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture> { }

public class SaveCategoryTestFixture : CategoryUseCaseFixture
{
    public SaveCategoryInput GetValidInput()
        => new(Guid.NewGuid(),
                GetValidName(),
                GetDescription(),
                DateTime.Now,
                GetRandomBoolean()
            );

    public SaveCategoryInput GetInvalidInput()
        => new(Guid.NewGuid(),
                null,
                GetDescription(),
                DateTime.Now,
                GetRandomBoolean()
            );
}
