using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Interface;
using Todo.Models;

namespace TodoApp.Controllers
{
    //[Authorize]
    public class TodoController : Controller
    {
        public TodoController()
        {

        }

        // 在这里添加 Actions
        public async Task<IActionResult> Index()
        {
            IUserGrain user = Startup.SlioClient.GetGrain<IUserGrain>(1);
            var items = await user.GetTodoItemsAsync();

            var model = new TodoViewModel()
            {
                Items = items
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            //var successful = await _todoItemService.AddItemAsync(newItem);
            //if (!successful)
            //{
            //    return BadRequest("Could not add item.");
            //}

            return RedirectToAction("Index");
        }
    }
}
