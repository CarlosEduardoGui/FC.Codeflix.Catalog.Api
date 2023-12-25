
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SaveCategory;

[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest : IDisposable
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
        => _fixture = fixture;

    [Trait("Integration", "ElasticSearch - SaveCategory")]
    [Fact(DisplayName = nameof(SaveValidCategory_WhenInputIsValid_PersistsCategory))]
    public async Task SaveValidCategory_WhenInputIsValid_PersistsCategory()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id, ct: CancellationToken.None);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Trait("Integration", "ElasticSearch - SaveCategory")]
    [Fact(DisplayName = nameof(SaveValidCategory_WhenInputIsInvalid_ThrowsException))]
    public async Task SaveValidCategory_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var input = _fixture.GetInvalidInput();
        var expectedMessage = $"Name should not be empty or null.";

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should()
            .ThrowExactlyAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id, ct: CancellationToken.None);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
        => _fixture.DeleteElesticsearchAllDocument();
}
