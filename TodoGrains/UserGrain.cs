using System;
using Todo.Models;
using Todo.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans;
using Orleans.Providers;

namespace Todo.Server
{
    [StorageProvider(ProviderName="Mysql")]
    public class UserGrain : Grain<UserGrainState>, IUserGrain
    {
        public override Task OnActivateAsync()
        {
            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Id = Guid.NewGuid().ToString(), Title = "Todo01", DueAt = DateTime.Now.AddDays(3) });
            items.Add(new TodoItem() { Id = Guid.NewGuid().ToString(), Title = "Todo02", DueAt = DateTime.Now.AddDays(3) });

            if (State.items == null)
            {
                State.items = items.ToArray();
                base.WriteStateAsync();
            }

            return base.OnActivateAsync();
        }

        public Task<TodoItem[]> GetTodoItemsAsync()
        {
            return Task.FromResult(State.items);
        }

    }
}
