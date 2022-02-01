using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Services
{
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _httpClient;

        public ExternalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TodoItem> GetTodoItem(int id)
        {
            RestClient restClient = new RestClient(_httpClient, new RestClientOptions(_httpClient.BaseAddress));

            return await restClient.GetAsync<TodoItem>(new RestRequest($"/todos/{id}"));
        }
    }
}
