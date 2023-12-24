using FC.Codeflix.Catalog.Domain.Exceptions;

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
        var repository = Common.CategoryUseCaseFixture.GetMockRepository();
        var useCase = new UseCaseCategory.SaveCategory.SaveCategory(repository.Object);
        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.SaveAsync(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Once
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
        var repository = Common.CategoryUseCaseFixture.GetMockRepository();
        var useCase = new UseCaseCategory.SaveCategory.SaveCategory(repository.Object);
        var input = _fixture.GetInvalidInput();

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.SaveAsync(
            It.IsAny<DomainEntity.Category>(),
            It.IsAny<CancellationToken>()),
            Times.Never
        );
        await action.Should()
            .ThrowExactlyAsync<EntityValidationException>()
            .WithMessage($"Name should not be empty or null.");
    }
}
