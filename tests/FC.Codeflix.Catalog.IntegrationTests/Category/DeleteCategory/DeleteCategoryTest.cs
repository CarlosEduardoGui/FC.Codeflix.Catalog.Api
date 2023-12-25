using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.DeleteCategory;

[Collection(nameof(CategoryTestFixture))]
public class DeleteCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public DeleteCategoryTest(CategoryTestFixture fixture) 
        => _fixture = fixture;

    [Trait("Integration", "ElasticSearch - DeleteCategory")]
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivedExistingId_DeletesCategory))]
    public async Task DeleteCategory_WhenReceivedExistingId_DeletesCategory()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var categoriesExample = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var randomCategoryIdToDelete = new Random().Next(categoriesExample.Count);
        var input = new DeleteCategoryInput(categoriesExample[randomCategoryIdToDelete].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(input.Id);
        deletedCategory.Should().NotBeNull();
        deletedCategory.Found.Should().BeFalse();
    }

    [Trait("Integration", "ElasticSearch - DeleteCategory")]
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivedAndNonExistingId_ThrowsException))]
    public async Task DeleteCategory_WhenReceivedAndNonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var categoriesExample = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var randomCategoryId = Guid.NewGuid();
        var input = new DeleteCategoryInput(randomCategoryId);

        var action = async() => await mediator.Send(input, CancellationToken.None);

        await action.Should()
            .ThrowExactlyAsync<NotFoundException>()
            .WithMessage($"Category '{randomCategoryId}' not found.");
    }

    public void Dispose()
        => _fixture.DeleteElesticsearchAllDocument();
}
