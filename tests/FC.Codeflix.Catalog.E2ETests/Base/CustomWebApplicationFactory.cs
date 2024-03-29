﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Http;

namespace FC.Codeflix.Catalog.E2ETests.Base;
public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public readonly string BaseURL = "http://localhost:61000";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var enviroment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIROMENT", enviroment);
        builder.UseEnvironment(enviroment);
        builder.ConfigureServices(services =>
        {
            services.AddTransient<HttpMessageHandlerBuilder>(
                sp => new TestServerHttpMessageHandlerBuilder(Server)
            );
            services
                .AddCatalogClient()
                .ConfigureHttpClient(
                    client => client.BaseAddress = new Uri($"{BaseURL}/graphql")
                );
        });
        base.ConfigureWebHost(builder);
    }
}
