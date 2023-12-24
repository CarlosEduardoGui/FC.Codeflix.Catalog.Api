using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;
public class SearchCategoryInput : SearchListInput, IRequest<SearchListOutput<CategoryModelOutput>>
{
    public SearchCategoryInput(
        int page = 1,
        int perPage = 10,
        string search = "",
        string orderBy = "",
        SearchOrder order = SearchOrder.ASC
    )
        : base(page, perPage, search, orderBy, order)
    {
    }
}
