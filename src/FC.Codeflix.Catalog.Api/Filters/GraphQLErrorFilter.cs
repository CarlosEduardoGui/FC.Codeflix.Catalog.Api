﻿namespace FC.Codeflix.Catalog.Api.Filters;

public class GraphQLErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
        => error.WithMessage(error.Exception?.Message ?? "Unexpected error.");
}
