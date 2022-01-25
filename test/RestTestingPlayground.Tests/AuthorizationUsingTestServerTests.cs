using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Tests
{
    [TestFixture]
    public class AuthorizationUsingTestServerTests
    {
        private TestServer _testServer;

        private Mock<IAuthorizationService> _authorizationServiceMock;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _authorizationServiceMock = new Mock<IAuthorizationService>();

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.Replace(ServiceDescriptor.Singleton(_authorizationServiceMock.Object));
                });

            _testServer = new TestServer(webHostBuilder);

            _httpClient = _testServer.CreateClient();
            _restClient = new RestClient(_httpClient, new RestClientOptions(_httpClient.BaseAddress));
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [Test]
        public async Task Get()
        {
            _authorizationServiceMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "todos"))
                .ReturnsAsync(AuthorizationResult.Success());

            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("/authorizedtodos/1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(
                new TodoItem
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Test",
                    Completed = false
                });
        }
    }
}
