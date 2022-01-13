using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("externaltodos")]
    public class ExternalTodoController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<TodoItem> Get(int id)
        {
            string externalUrl = Environment.GetEnvironmentVariable("EXTERNAL_SERVICE_URL")
                ?? "https://jsonplaceholder.typicode.com";

            RestClient restClient = new RestClient(externalUrl);

            return await restClient.GetAsync<TodoItem>(new RestRequest($"/todos/{id}"));
        }
    }
}
