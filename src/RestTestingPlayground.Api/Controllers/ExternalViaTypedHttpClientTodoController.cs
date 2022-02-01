using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestTestingPlayground.Api.Models;
using RestTestingPlayground.Api.Services;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("externalviatypedhttpclienttodos")]
    public class ExternalViaTypedHttpClientTodoController
    {
        private readonly IExternalService _externalService;

        public ExternalViaTypedHttpClientTodoController(IExternalService externalService)
        {
            _externalService = externalService;
        }

        [HttpGet("{id}")]
        public async Task<TodoItem> Get(int id)
        {
            return await _externalService.GetTodoItem(id);
        }
    }
}
