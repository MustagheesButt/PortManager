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
    public class ItemController : Controller
    {
        [Route("/Item")]
        public IActionResult Index()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            ViewData["items"] = Item.GetAllByTrader(trader_id);

            return View("Index");
        }

        public IActionResult Add()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string manufacturer, int price)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            int trader_id = (int)HttpContext.Session.GetInt32("user_id");

            Item item = new Item(-1, name, trader_id, price, Manufacturer: manufacturer, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now);
            Item.Add(item);

            return RedirectToAction("Index", "Item");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            Item item = Item.GetOne(id);
            ViewData["item"] = item;

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, string name, string manufacturer, int price)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            // TODO verify item belongs to the current_user/trader

            Item item = Item.GetOne(id);
            item.Name = name;
            item.Manufacturer = manufacturer;
            item.Price = price;
            item.UpdatedAt = DateTime.Now;

            Item.Update(item);

            return RedirectToAction("Index", "Item");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            Item.DeleteById(id);
            return RedirectToAction("Index", "Item");
        }
    }
}
