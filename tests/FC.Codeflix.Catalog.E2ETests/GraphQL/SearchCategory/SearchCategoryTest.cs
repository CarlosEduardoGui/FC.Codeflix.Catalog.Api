using RepositoryDTO = FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.Infra.Data.ES;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.SearchCategory;

[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest : IDisposable
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
        => _fixture = fixture;

    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Category] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var elasticClient = _fixture.ElasticClient;
        var categoryNamesList = new List<string>() {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };
        var examples = _fixture.GetCategoriesModelList(categoryNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);

        var output = await _fixture.GraphQLClient.SearchCategory
            .ExecuteAsync(page, perPage, search, "", SearchOrder.Asc, CancellationToken.None);

        output.Data!.Categories.Should().NotBeNull();
        output.Data!.Categories.Items.Should().NotBeNull();
        output.Data!.Categories.CurrentPage.Should().Be(page);
        output.Data!.Categories.PerPage.Should().Be(perPage);
        output.Data!.Categories.Total.Should().Be(expectedTotalCount);
        output.Data!.Categories.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Data!.Categories.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Name.Should().Be(expected!.Name);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    [Trait("E2E/GraphQL", "SearchCategory")]
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
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetCategoriesModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);
        var page = 1;
        var perPage = examples.Count;
        var directionGraphQL = direction == "asc"
            ? SearchOrder.Asc
            : SearchOrder.Desc;
        var directionRepository = direction == "asc"
            ? RepositoryDTO.SearchOrder.ASC
            : RepositoryDTO.SearchOrder.DESC;
        var expectedList = _fixture.CloneCategoryListOrdered(examples, orderBy, directionRepository);

        var output = await _fixture
            .GraphQLClient
            .SearchCategory
            .ExecuteAsync(
                page,
                perPage,
                "",
                orderBy,
                directionGraphQL,
                CancellationToken.None
            );

        output.Data.Should().NotBeNull();
        output.Data!.Categories.Should().NotBeNull();
        output.Data!.Categories.Items.Should().NotBeNull();
        output.Data!.Categories.CurrentPage.Should().Be(page);
        output.Data!.Categories.PerPage.Should().Be(perPage);
        output.Data!.Categories.Total.Should().Be(examples.Count);
        output.Data!.Categories.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Data!.Categories.Items.Count; i++)
        {
            var item = output.Data.Categories.Items[i];
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
