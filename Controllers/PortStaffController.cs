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
    public class PortStaffController : Controller
    {
        private readonly ILogger<StaticController> _logger;

        public PortStaffController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Register(String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return RedirectToAction("/Register");
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return RedirectToAction("/Register");
            }

            Models.User user = Models.User.GetUserByEmail(Email);
            
            Models.User.Add_Trader(FirstName , LastName , Email , Password , 2);
            
            return RedirectToAction("Dashboard", "PortStaff");
        }

        [Route("/PortStaff")]
        public IActionResult Dashboard()
        {
            ViewData["ships"] = Models.Ship.GetShipsByTrader(1);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
