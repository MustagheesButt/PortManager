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
    public class AdminController : Controller
    {
        private readonly ILogger<StaticController> _logger;

        public AdminController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        [Route("/Admin")]
        public IActionResult Dashboard()
        {
            ViewData["users"] = Models.User.GetUsers();
            return View();
        }

        public IActionResult Ships()
        {
            ViewData["ships"] = Models.Ship.GetShips();
            return View();
        }

        public IActionResult CustomDuties()
        {
            ViewData["duties"] = Models.CustomDuty.GetCustomDuties();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
