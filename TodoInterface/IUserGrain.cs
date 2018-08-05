using System;
using System.Threading.Tasks;
using Orleans;
using Todo.Models;

namespace Todo.Interface
{
    public interface IUserGrain : IGrainWithIntegerKey
    {
        Task<TodoItem[]> GetTodoItemsAsync();
    }
}
