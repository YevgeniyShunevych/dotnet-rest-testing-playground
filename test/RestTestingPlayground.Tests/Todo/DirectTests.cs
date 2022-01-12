using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Tests.Todo
{
    [TestFixture]
    public class DirectTests
    {
        private RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _restClient = new RestClient("https://jsonplaceholder.typicode.com/todos");
        }

        [Test]
        public async Task Get()
        {
            var response = await _restClient.ExecuteGetAsync<TodoItem>(new RestRequest("1"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(
                new TodoItem
                {
                    Id = 1,
                    UserId = 1,
                    Title = "delectus aut autem",
                    Completed = false
                });
        }
    }
}
