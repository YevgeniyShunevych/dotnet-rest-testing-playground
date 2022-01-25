using Microsoft.AspNetCore.Mvc;
using RestTestingPlayground.Api.Authorization;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("authorizedtodos")]
    [CustomAuthorization("todos")]
    public class AuthorizedTodoController : ControllerBase
    {
        [HttpGet("{id}")]
        public TodoItem Get(int id)
        {
            return new TodoItem
            {
                Id = id,
                UserId = 1,
                Title = "Test",
                Completed = false
            };
        }
    }
}
