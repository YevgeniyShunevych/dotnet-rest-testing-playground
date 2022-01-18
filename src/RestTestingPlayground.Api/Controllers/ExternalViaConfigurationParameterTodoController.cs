using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("externalviaconftodos")]
    public class ExternalViaConfigurationParameterTodoController
    {
        private readonly IConfiguration _configuration;

        public ExternalViaConfigurationParameterTodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<TodoItem> Get(int id)
        {
            string externalUrl = _configuration.GetValue<string>("ExternalServiceUrl");

            RestClient restClient = new RestClient(externalUrl);

            return await restClient.GetAsync<TodoItem>(new RestRequest($"/todos/{id}"));
        }
    }
}
