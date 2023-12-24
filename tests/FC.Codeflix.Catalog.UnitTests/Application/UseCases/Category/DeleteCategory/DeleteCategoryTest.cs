using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(CategoryUseCaseFixture))]
public class DeleteCategoryTest
{
    private readonly CategoryUseCaseFixture _fixture;

    public DeleteCategoryTest(CategoryUseCaseFixture fixture)
        => _fixture = fixture;

    [Trait("Application", "UseCases - DeleteCategory")]
    [Fact(DisplayName = nameof(DeleteCategory))]
    public async Task DeleteCategory()
    {
        var repository = CategoryUseCaseFixture.GetMockRepository();
        var useCase = new UseCaseCategory.DeleteCategory.DeleteCategory(repository);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        await repository
            .Received(1)
            .DeleteAsync(input.Id, Arg.Any<CancellationToken>());
    }
}
