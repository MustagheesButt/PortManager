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
    public class ShipController : Controller
    {
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
            Ship ship = Ship.GetShip(id);
            ViewData["ship"] = ship;

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, string hin, string nick_name, int alloc_birth, int alloc_term)
        {
            Ship ship = Ship.GetShip(id);
            ship.HIN = hin;
            ship.NickName = nick_name;
            ship.AllocatedBirth = alloc_birth;
            ship.AllocatedTerminal = alloc_term;
     
            Ship.Update(ship);
            return RedirectToAction("Dashboard", "Trader");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Ship.DeleteById(id);
            return RedirectToAction("Dashboard", "Trader");
        }
    }
}
