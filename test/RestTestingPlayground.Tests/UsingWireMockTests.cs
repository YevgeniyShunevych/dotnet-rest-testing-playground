using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace RestTestingPlayground.Tests
{
    [TestFixture]
    public class UsingWireMockTests
    {
        private WireMockServer _mockServer;

        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _mockServer = WireMockServer.Start();
            _restClient = new RestClient($"{_mockServer.Urls[0]}");
        }

        [TearDown]
        public void TearDown()
        {
            _mockServer.Dispose();
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

            _mockServer
                .Given(Request.Create().WithPath("/todos/1").UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(setupItem));

            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("/todos/1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(setupItem);
        }
    }
}
