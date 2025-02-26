using UriLix.API.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest;

[Collection(nameof(WebApplicationTestCollection))]
public abstract class BaseWebApplicationTest(
    IntegrationTestWebApplication<Program> _factory,
    ITestOutputHelper _outputHelper)
{
    protected readonly HttpClient httpClient = _factory.CreateClient();
    protected readonly IntegrationTestWebApplication<Program> factory = _factory;
    protected readonly ITestOutputHelper outputHelper = _outputHelper;
}
