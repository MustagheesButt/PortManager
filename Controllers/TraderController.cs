using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortManager.Models;

namespace PortManager.Controllers
{
    public class TraderController : Controller
    {
        private readonly ILogger<StaticController> _logger;

        public TraderController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        [Route("/Trader")]
        public IActionResult Dashboard()
        {
            ViewData["ships"] = Models.Ship.GetShipsByTrader(1);
            return View();
        }

        public IActionResult AddShip()
        {
            return View();
        }

        [HttpGet("Trader/UpdateShip/{ship_id}")]
        public IActionResult UpdateShip(int ship_id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
