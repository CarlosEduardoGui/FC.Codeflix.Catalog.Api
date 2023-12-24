using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SaveCategory;

[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
        => _fixture = fixture;

    [Trait("Application", "UseCases - SaveCategory")]
    [Fact(DisplayName = nameof(SaveValidCategory))]
    public async Task SaveValidCategory()
    {
        var repository = CategoryUseCaseFixture.GetMockRepository();
        var useCase = new UseCaseCategory.SaveCategory.SaveCategory(repository);
        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1).SaveAsync(
            Arg.Any<DomainEntity.Category>(),
            Arg.Any<CancellationToken>()
        );
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().Be(input.CreatedAt);
        output.IsActive.Should().Be(input.IsActive);
    }

    [Trait("Application", "UseCases - SaveCategory")]
    [Fact(DisplayName = nameof(SaveInvalidCategory))]
    public async Task SaveInvalidCategory()
    {
        var repository = CategoryUseCaseFixture.GetMockRepository();
        var useCase = new UseCaseCategory.SaveCategory.SaveCategory(repository);
        var input = _fixture.GetInvalidInput();

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await repository.DidNotReceive().SaveAsync(
            Arg.Any<DomainEntity.Category>(),
            Arg.Any<CancellationToken>()
        );
        await action.Should()
            .ThrowExactlyAsync<EntityValidationException>()
            .WithMessage($"Name should not be empty or null.");
    }
}
