﻿using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace RestTestingPlayground.Tests.Todo
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
            _restClient = new RestClient($"{_mockServer.Urls[0]}/todos");
        }

        [TearDown]
        public void TearDown()
        {
            _mockServer.Stop();
        }

        [Test]
        public async Task Get()
        {
            var setupItem = new TodoItem
            {
                Id = 1,
                UserId = 1,
                Title = "delectus aut autem",
                Completed = false
            };

            _mockServer
                .Given(Request.Create().WithPath("/todos/1").UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(setupItem));

            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(setupItem);
        }
    }
}