using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var useCase = new UseCaseCategory.DeleteCategory.DeleteCategory(repository.Object);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.DeleteAsync(
            input.Id,
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
