using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Api.Categories;

public class CategoryPayload
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public static CategoryPayload FromCategoryModelOutput(CategoryModelOutput categoryModelOutput)
        => new()
        {
            Id = categoryModelOutput.Id,
            Name = categoryModelOutput.Name,
            Description = categoryModelOutput.Description,
            CreatedAt = categoryModelOutput.CreatedAt,
            IsActive = categoryModelOutput.IsActive
        };
}
