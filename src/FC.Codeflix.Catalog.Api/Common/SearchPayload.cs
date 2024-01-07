namespace FC.Codeflix.Catalog.Api.Common;

public class SearchPayload<T> where T : class
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; set; }
}
