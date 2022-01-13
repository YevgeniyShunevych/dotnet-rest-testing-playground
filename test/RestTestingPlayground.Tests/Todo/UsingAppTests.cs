using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Tests.Todo
{
    [TestFixture]
    public class UsingAppTests
    {
        private WebApplicationFactory<Program> _applicationFactory;

        private HttpClient _httpClient;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _applicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                });

            _httpClient = _applicationFactory.CreateClient();
            _restClient = new RestClient(_httpClient, new RestClientOptions(_httpClient.BaseAddress));
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _applicationFactory.Dispose();
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
