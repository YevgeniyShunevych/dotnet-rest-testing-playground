using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("externalvianamedhttpclienttodos")]
    public class ExternalViaNamedHttpClientTodoController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalViaNamedHttpClientTodoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{id}")]
        public async Task<TodoItem> Get(int id)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ExternalService1");

            RestClient restClient = new RestClient(httpClient, new RestClientOptions(httpClient.BaseAddress));

            return await restClient.GetAsync<TodoItem>(new RestRequest($"/todos/{id}"));
        }
    }
}
