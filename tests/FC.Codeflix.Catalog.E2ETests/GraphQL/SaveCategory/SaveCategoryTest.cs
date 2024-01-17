using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.SaveCategory;

[Collection(nameof(CategoryTestFixture))]
public class SaveCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public SaveCategoryTest(CategoryTestFixture fixture)
        => _fixture = fixture;

    [Trait("E2E/GraphQL", "SaveCategory")]
    [Fact(DisplayName = nameof(SaveValidCategory_WhenInputIsValid_PersistsCategory))]
    public async Task SaveValidCategory_WhenInputIsValid_PersistsCategory()
    {
        var serviceProvider = _fixture.WebbAppFactory.Services;
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var input = new SaveCategoryInput()
        {
            Id = Guid.NewGuid(),
            Name = "Action",
            Description = "Action Test",
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = true
        };

        var output = await _fixture.GraphQLClient.SaveCategory.ExecuteAsync(input, CancellationToken.None);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id, ct: CancellationToken.None);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt.DateTime);
        output.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Data!.SaveCategory.Id.Should().Be(input.Id);
        output.Data!.SaveCategory.Name.Should().Be(input.Name);
        output.Data!.SaveCategory.Description.Should().Be(input.Description);
        output.Data!.SaveCategory.IsActive.Should().Be(input.IsActive);
        output.Data!.SaveCategory.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Trait("Integration", "ElasticSearch - SaveCategory")]
    [Fact(DisplayName = nameof(SaveValidCategory_WhenInputIsInvalid_ThrowsException))]
    public async Task SaveValidCategory_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = _fixture.WebbAppFactory.Services;
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var expectedMessage = $"Name should not be empty or null.";
        var input = new SaveCategoryInput()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = "Action Test",
            CreatedAt = DateTime.Now.Date,
            IsActive = true
        };

        var output = await _fixture.GraphQLClient.SaveCategory.ExecuteAsync(input, CancellationToken.None);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id, ct: CancellationToken.None);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
        => _fixture.DeleteElesticsearchAllDocument();
}

