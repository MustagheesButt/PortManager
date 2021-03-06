using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortManager.Models;

namespace PortManager.Controllers
{
    public class CustomDutyController : Controller
    {
        [Route("/CustomDuties")]
        public IActionResult Index()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            //int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            ViewData["custom_duties"] = CustomDuty.GetAll();

            return View("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, int quantity, int price, int ship_id)
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");

            Item item = new Item(-1, name, trader_id, quantity, price, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now);
            Item.Add(item, ship_id);

            return RedirectToAction("Dashboard", "Item");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Item item = Item.GetOne(id);
            ViewData["item"] = item;

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, string name, int quantity, int price, int ship_id)
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");

            // TODO verify item belongs to the current_user/trader

            Item item = Item.GetOne(id);
            item.Name = name;
            item.Quantity = quantity;
            item.Price = price;
            item.UpdatedAt = DateTime.Now;

            Item.Update(item);

            return RedirectToAction("Dashboard", "Item");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            CustomDuty.DeleteById(id);
            return RedirectToAction("Dashboard", "Trader");
        }
    }
}
