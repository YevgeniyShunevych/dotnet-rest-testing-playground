using System;
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
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace RestTestingPlayground.Tests.Todo
{
    [TestFixture]
    public class UsingAppWithWireMockTests
    {
        private WireMockServer _externalServiceMock;

        private TestServer _testServer;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _externalServiceMock = WireMockServer.Start();

            Environment.SetEnvironmentVariable("EXTERNAL_SERVICE_URL", _externalServiceMock.Urls[0]);

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            _testServer = new TestServer(webHostBuilder);

            _httpClient = _testServer.CreateClient();
            _restClient = new RestClient(_httpClient, new RestClientOptions(_httpClient.BaseAddress));
        }

        [TearDown]
        public void TearDown()
        {
            _externalServiceMock.Dispose();
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [Test]
        public async Task Get()
        {
            var setupItem = new TodoItem
            {
                Id = 3,
                UserId = 9,
                Title = "Some title",
                Completed = true
            };

            _externalServiceMock
                .Given(Request.Create().WithPath("/todos/1").UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(setupItem));

            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("/externaltodos/1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(setupItem);
        }
    }
}
