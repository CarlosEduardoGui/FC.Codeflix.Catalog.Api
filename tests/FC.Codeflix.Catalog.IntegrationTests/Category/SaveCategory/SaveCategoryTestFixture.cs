using FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SaveCategory;
[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture> { }

public class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
        => new(Guid.NewGuid(),
                DataGenerator.GetValidCategoryName(),
                DataGenerator.GetValidCategoryDescription(),
                DateTime.Now,
                DataGenerator.GetRandomBoolean()
            );

    public SaveCategoryInput GetInvalidInput()
        => new(Guid.NewGuid(),
                null,
                DataGenerator.GetValidCategoryDescription(),
                DateTime.Now,
                DataGenerator.GetRandomBoolean()
            );
}
