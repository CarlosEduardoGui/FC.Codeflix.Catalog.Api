using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;

    [Trait("Domain", "Category - Aggregates")]
    [Fact(DisplayName = nameof(Instantiate))]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().BeTrue();
    }

    [Trait("Domain", "Category - Aggregates")]
    [Theory(DisplayName = nameof(InstantiateWithActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt,
            isActive);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().Be(isActive);
    }

    [Trait("Domain", "Category - Aggregates")]
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () =>
        {
            DomainEntity.Category category = new(
            validCategory.Id,
            name,
            validCategory.Description,
            validCategory.CreatedAt);
        };

        var exception = Assert.Throws<EntityValidationException>(() => action());

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null.");
    }

    [Trait("Domain", "Category - Aggregates")]
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () =>
        {
            DomainEntity.Category category = new(
            validCategory.Id,
            validCategory.Name,
            null!,
            validCategory.CreatedAt);
        };

        var exception = Assert.Throws<EntityValidationException>(() => action());

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null.");
    }

    [Trait("Domain", "Category - Aggregates")]
    [Fact(DisplayName = nameof(InstantiateErrorWhenIdIsEmpty))]
    public void InstantiateErrorWhenIdIsEmpty()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () =>
        {
            DomainEntity.Category category = new(
            Guid.Empty,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt);
        };

        var exception = Assert.Throws<EntityValidationException>(() => action());

        action.Should().Throw<EntityValidationException>().WithMessage("Id should not be empty or null.");

    }
}
