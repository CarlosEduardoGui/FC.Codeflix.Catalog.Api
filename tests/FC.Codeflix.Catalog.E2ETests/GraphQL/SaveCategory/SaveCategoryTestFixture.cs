namespace FC.Codeflix.Catalog.E2ETests.GraphQL.SaveCategory;

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture> { }

public class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
        => new()
        {
            Id = Guid.NewGuid(),
            Name = DataGenerator.GetValidCategoryName(),
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };

    public SaveCategoryInput GetInvalidInput()
        => new()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };
}
