using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.Dtos;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;
using SearchInput = FC.Codeflix.Catalog.Domain.Repositories.Dtos.SearchInput;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly IElasticClient _client;

    public CategoryRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync<CategoryModel>(id, ct: cancellationToken);

        if (response.Result == Result.NotFound)
            throw new NotFoundException(string.Format(ConstMessages.NotFound, "Category", id.ToString()));
    }

    public async Task SaveAsync(Category entity, CancellationToken cancellationToken)
    {
        var model = CategoryModel.FromCategory(entity);
        await _client.IndexDocumentAsync(model, cancellationToken);
    }

    public Task<SearchOutput<Category>> SearchAsync(SearchInput intput, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
