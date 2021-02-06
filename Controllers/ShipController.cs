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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Ship ship = Ship.GetShip(id);
            ViewData["ship"] = ship;

            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Ship.DeleteById(id);
            return RedirectToAction("Dashboard", "Trader");
        }
    }
}
