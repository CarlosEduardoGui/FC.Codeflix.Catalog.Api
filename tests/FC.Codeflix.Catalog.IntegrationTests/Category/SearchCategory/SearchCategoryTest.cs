using FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.Infra.Data.ES;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SearchCategory;

[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest : IDisposable
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
        => _fixture = fixture;

    [Trait("Integration", "ElasticSearch - SearchCategory")]
    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidInput_ReturnFilteredList))]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Drama", 1, 5, 1, 1)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 5, 6)]
    [InlineData("Orthers", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCategory_WhenReceivesValidInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount
    )
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var categoriesNameList = new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Future",
            "Sci-fi",
            "Sci-fi Robots",
            "Sci-fi StarWars",
            "Sci-fi StarTrek"
        };
        var examples = _fixture.GetCategoriesModelList(categoriesNameList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);
        var input = new SearchCategoryInput(page: page, perPage: perPage, search: search);

        var output = await mediator!.Send(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedTotalCount);
        output.Items.Should().HaveCount(expectedItemsCount);
        foreach (var item in output.Items)
        {
            var expected = examples.FirstOrDefault(x => x.Id == item.Id);
            expected.Should().NotBeNull();
            item.Name.Should().Be(expected!.Name);
            item.Description.Should().Be(expected.Description);
            item.IsActive.Should().Be(expected.IsActive);
            item.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    [Trait("Integration", "ElasticSearch - SearchCategory")]
    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidInput_ReturnsOrderedList))]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "desc")]
    public async Task SearchCategory_WhenReceivesValidInput_ReturnsOrderedList(string orderBy, string direction)
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        var examples = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);
        var input = new SearchCategoryInput(
            page: 1,
            perPage: examples.Count,
            orderBy: orderBy,
            order: direction == "asc" ? SearchOrder.ASC : SearchOrder.DESC
        );
        var expectedList = SearchCategoryTestFixture.CloneCategoryListOrdered(examples, orderBy, input.Order);

        var output = await mediator!.Send(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(examples.Count);
        output.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Items.Count; i++)
        {
            var item = output.Items[i];
            var expected = expectedList[i];
            item.Id.Should().Be(expected.Id);
            item.Name.Should().Be(expected!.Name);
            item.Description.Should().Be(expected.Description);
            item.IsActive.Should().Be(expected.IsActive);
            item.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    public void Dispose()
        => _fixture.DeleteElesticsearchAllDocument();
}
