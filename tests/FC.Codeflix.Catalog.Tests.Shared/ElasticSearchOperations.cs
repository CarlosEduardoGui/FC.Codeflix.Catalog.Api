using Elasticsearch.Net;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;

namespace FC.Codeflix.Catalog.Tests.Shared;
public static class ElasticSearchOperations
{
    private const string SUFFIX_KEYWORD = "keyword";

    public static async Task CreateCategoryIndex(IElasticClient esClient)
    {
        DeleteCategoryIndex(esClient);

        await esClient.Indices.CreateAsync(
            ElasticsearchIndices.Category,
            c => c
                .Map<CategoryModel>(m => m
                    .Properties(ps => ps
                        .Keyword(t => t
                            .Name(category => category.Id)
                        )
                        .Text(t => t
                            .Name(category => category.Name)
                            .Fields(fs => fs
                                .Keyword(k => k
                                    .Name(category => category.Name.Suffix(SUFFIX_KEYWORD)))
                            )
                        )
                        .Text(t => t
                            .Name(category => category.Description)
                        )
                        .Boolean(b => b
                            .Name(category => category.IsActive)
                        )
                        .Date(d => d
                            .Name(category => category.CreatedAt)
                        )
                    )
                )
        );
    }

    public static void DeleteAllCategoriesDocuments(IElasticClient esClient)
     => esClient.DeleteByQuery<CategoryModel>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed)
        );

    public static void DeleteCategoryIndex(IElasticClient esClient)
        => esClient.Indices.Delete(ElasticsearchIndices.Category);
}
