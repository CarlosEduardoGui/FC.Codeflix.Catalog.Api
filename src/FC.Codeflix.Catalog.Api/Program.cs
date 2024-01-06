using FC.Codeflix.Catalog.Api;
using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Api.Categories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddUseCases()
    .AddElasticSearch(builder.Configuration)
    .AddRepositories()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<CategoryMutations>();

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
