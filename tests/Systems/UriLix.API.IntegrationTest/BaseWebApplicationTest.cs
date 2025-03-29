using Microsoft.AspNetCore.Mvc.Testing;
using UriLix.API.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest;

[Collection(nameof(WebApplicationTestCollection))]
public abstract class BaseWebApplicationTest(
    IntegrationTestWebApplication<Program> factory,
    ITestOutputHelper outputHelper)
{

    protected readonly HttpClient httpClient = factory.CreateClient(
        new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    protected readonly IntegrationTestWebApplication<Program> factory = factory;
    protected readonly ITestOutputHelper outputHelper = outputHelper;
}
