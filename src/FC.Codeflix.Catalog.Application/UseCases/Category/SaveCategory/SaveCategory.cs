using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repositories;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;


namespace FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
public class SaveCategory : ISaveCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public SaveCategory(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<CategoryModelOutput> Handle(SaveCategoryInput request, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(
            request.Id,
            request.Name,
            request.Description,
            request.CreatedAt,
            request.IsActive
        );

        await _categoryRepository.SaveAsync(category, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
