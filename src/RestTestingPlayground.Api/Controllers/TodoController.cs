using Microsoft.AspNetCore.Mvc;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public TodoItem Get(int id)
        {
            return new TodoItem
            {
                Id = id,
                UserId = 1,
                Title = "delectus aut autem",
                Completed = false
            };
        }
    }
}
