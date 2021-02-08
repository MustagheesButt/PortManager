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
    public class TraderController : Controller
    {
        private readonly ILogger<StaticController> _logger;

        public TraderController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Register(String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return Redirect("/Register");
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return Redirect("/Register");
            }
            
            Models.User.Add_Trader(FirstName , LastName , Email , Password , 1);

            Models.User user = Models.User.GetUserByEmail(Email);

            HttpContext.Session.SetInt32("user_id", user.id);

            return RedirectToAction("Dashboard", "Trader");
        }

        [Route("/Trader")]
        public IActionResult Dashboard()
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            ViewData["ships"] = Ship.GetShipsByTrader(trader_id);
            //Console.WriteLine(trader_id);
            return View("Dashboard" , trader_id);
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

        [HttpPost]
        public IActionResult AddShipForm(string hin , string nick_name , int alloc_birth , int alloc_term)
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");

            Ship ship = new Ship(hin , trader_id , nick_name , alloc_birth , alloc_term);
            Ship.AddShip(ship);

            return RedirectToAction("Dashboard", "Trader");
        }

        [HttpGet("Trader/DeleteShip/{ship_id}")]
        public IActionResult DeleteShip(int ship_id)
        {
            PortManager.Models.Ship.DeleteShip(ship_id);
            return RedirectToAction("Dashboard", "Trader");
        }

        [HttpGet("Trader/EditShip/{ship_id}")]
        public IActionResult EditShip(int ship_id)
        {
            return View("EditShip" , ship_id);
        }

        [HttpPost]
        public IActionResult EditShipForm(string hin , string nick_name , int alloc_birth , int alloc_term , int ship_id )
        {
            Ship ship = new Ship(ship_id ,hin , nick_name , alloc_birth , alloc_term);
            PortManager.Models.Ship.EditShip(ship);
            return RedirectToAction("Dashboard", "Trader");
        }
    }
}
