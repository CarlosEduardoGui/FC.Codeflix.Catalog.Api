using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Categories;

public class CategoryMutations
{
    public async Task<CategoryModelOutput> SaveCategoryAsync(
        SaveCategoryInput input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var output = await mediator.Send(input, cancellationToken);

        return output;
    }
}
