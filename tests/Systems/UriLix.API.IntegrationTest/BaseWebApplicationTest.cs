using Microsoft.AspNetCore.Mvc.Testing;
using UriLix.API.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest;

[Collection(nameof(WebApplicationTestCollection))]
public abstract class BaseWebApplicationTest(
    IntegrationTestWebApplication<Program> _factory,
    ITestOutputHelper _outputHelper)
{

    protected readonly HttpClient httpClient = _factory.CreateClient(
        new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    protected readonly IntegrationTestWebApplication<Program> factory = _factory;
    protected readonly ITestOutputHelper outputHelper = _outputHelper;
}
