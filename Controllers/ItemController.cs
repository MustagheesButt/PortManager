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
        public IActionResult Dashboard()
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            ViewData["items"] = Item.GetAllByTrader(trader_id);

            return View("Dashboard", trader_id);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string hin, string nick_name, int alloc_birth, int alloc_term)
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");

            Ship ship = new Ship(hin, trader_id, nick_name, alloc_birth, alloc_term);
            Ship.Add(ship);

            return RedirectToAction("Dashboard", "Trader");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Item item = Item.GetOne(id);
            ViewData["item"] = item;

            return View();
        }

        [HttpPost]
        public IActionResult Update(string hin, string nick_name, int alloc_birth, int alloc_term, int ship_id)
        {
            Ship ship = new Ship(ship_id, hin, nick_name, alloc_birth, alloc_term);
            Ship.Update(ship);
            return RedirectToAction("Dashboard", "Trader");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Item.DeleteById(id);
            return RedirectToAction("Dashboard", "Trader");
        }
    }
}
