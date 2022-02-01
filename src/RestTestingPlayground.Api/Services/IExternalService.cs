using System.Threading.Tasks;
using RestTestingPlayground.Api.Models;

namespace RestTestingPlayground.Api.Services
{
    public interface IExternalService
    {
        Task<TodoItem> GetTodoItem(int id);
    }
}