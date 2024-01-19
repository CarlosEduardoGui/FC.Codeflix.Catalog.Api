using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.DeleteCategory;

[Collection(nameof(CategoryTestFixture))]
public class DeleteCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public DeleteCategoryTest(CategoryTestFixture fixture) => 
        _fixture = fixture;

    [Trait("E2E/GraphQL", "DeleteCategory")]
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivedExistingId_DeletesCategory))]
    public async Task DeleteCategory_WhenReceivedExistingId_DeletesCategory()
    {
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var randomCategoryIdToDelete = new Random().Next(categoriesExample.Count);
        var id = categoriesExample[randomCategoryIdToDelete].Id;

        var output = await _fixture
            .GraphQLClient
            .DeleteCategory
            .ExecuteAsync(id, CancellationToken.None);

        output.Data.Should().NotBeNull();
        output.Data!.DeleteCategory.Should().BeTrue();
        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(id);
        deletedCategory.Should().NotBeNull();
        deletedCategory.Found.Should().BeFalse();
    }

    [Trait("E2E/GraphQL", "DeleteCategory")]
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivedAndNonExistingId_ReturnsErrors))]
    public async Task DeleteCategory_WhenReceivedAndNonExistingId_ReturnsErrors()
    {
        var elasticClient = _fixture.ElasticClient;
        var categoriesExample = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var randomCategoryId = Guid.NewGuid();
        var expectedErrorMessage = $"Category '{randomCategoryId}' not found.";

        var output = await _fixture
            .GraphQLClient
            .DeleteCategory
            .ExecuteAsync(randomCategoryId, CancellationToken.None);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedErrorMessage);
    }

    public void Dispose()
        => _fixture.DeleteElesticsearchAllDocument();
}
