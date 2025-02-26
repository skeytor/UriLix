namespace UriLix.API.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(WebApplicationTestCollection))]
public class WebApplicationTestCollection : ICollectionFixture<IntegrationTestWebApplication<Program>>;
