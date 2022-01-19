using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("externalviahttpclientfactorytodos")]
    public class ExternalViaHttpClientFactoryTodoController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalViaHttpClientFactoryTodoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{id}")]
        public async Task<TodoItem> Get(int id)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient("ExternalService1"))
            {
                RestClient restClient = new RestClient(httpClient, new RestClientOptions(httpClient.BaseAddress));

                return await restClient.GetAsync<TodoItem>(new RestRequest($"/todos/{id}"));
            }
        }
    }
}
