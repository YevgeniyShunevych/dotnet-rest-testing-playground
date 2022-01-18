using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Tests
{
    public class UsingKestrelServerTests
    {
        private IWebHost _webHost;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _webHost = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();

            _webHost.StartAsync();

            var webHostUrls = _webHost.ServerFeatures
                .Get<IServerAddressesFeature>()
                .Addresses;

            var testUrl = webHostUrls.First();

            _httpClient = new HttpClient { BaseAddress = new Uri(testUrl) };

            _restClient = new RestClient(_httpClient, new RestClientOptions(_httpClient.BaseAddress));
        }

        [TearDown]
        public async Task TearDown()
        {
            _httpClient.Dispose();

            await _webHost.StopAsync();
            _webHost.Dispose();
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
