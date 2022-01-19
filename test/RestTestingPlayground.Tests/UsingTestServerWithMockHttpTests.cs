using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api;
using RestTestingPlayground.Api.Models;
using RichardSzalay.MockHttp;

namespace RestTestingPlayground.Tests
{
    [TestFixture]
    public class UsingTestServerWithMockHttpTests
    {
        private const string ExternalServiceMockUrl = "https://extserv.org";

        private MockHttpMessageHandler _externalHttpMock;

        private TestServer _testServer;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _externalHttpMock = new MockHttpMessageHandler();

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddHttpClient("ExternalService1")
                        .ConfigurePrimaryHttpMessageHandler(() => _externalHttpMock);
                })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["ExternalServiceUrl"] = ExternalServiceMockUrl
                        });
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
            var setupItem = new TodoItem
            {
                Id = 3,
                UserId = 9,
                Title = Guid.NewGuid().ToString(),
                Completed = true
            };

            _externalHttpMock.Expect(HttpMethod.Get, ExternalServiceMockUrl + "/todos/1")
                .Respond("application/json", JsonConvert.SerializeObject(setupItem));

            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("/externalviahttpclientfactorytodos/1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(setupItem);

            // Additionally we can verify that expected requests were executed.
            _externalHttpMock.VerifyNoOutstandingExpectation();
        }
    }
}
