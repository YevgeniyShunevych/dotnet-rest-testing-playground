using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Tests.Todo
{
    [TestFixture]
    public class UsingAppTests
    {
        private TestServer _testServer;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

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
            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("/todos/1"));

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
