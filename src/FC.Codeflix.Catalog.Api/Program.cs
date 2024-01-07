using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Api.Categories;
using FC.Codeflix.Catalog.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddUseCases()
    .AddElasticSearch(builder.Configuration)
    .AddRepositories()
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<CategoryQueries>()
    .AddTypeExtension<CategoryMutations>()
    .AddErrorFilter<GraphQLErrorFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();
