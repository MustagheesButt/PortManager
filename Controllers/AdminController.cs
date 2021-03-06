using System;
using System.Diagnostics;
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
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            ViewData["users"] = Models.User.GetUsers();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("Admin/AddTrader")]
        public IActionResult AddTrader()
        {
            return View("AddTrader");
        }

        [HttpGet("Admin/AddStaff")]
        public IActionResult AddStaff()
        {
            return View("AddStaff");
        }

        [HttpGet("Admin/AddAdmin")]
        public IActionResult AddAdmin()
        {
            return View("AddAdmin");
        }

        [HttpPost]
        public IActionResult AddTraderForm(String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return RedirectToAction("AddTrader", "Admin");
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return RedirectToAction("AddTrader", "Admin");
            }
            
            Models.User.Add_User(new Models.User(-1, FirstName, LastName, Email, 0, PasswordHash: Password, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now));

            return RedirectToAction("Dashboard", "Admin");

        }

        [HttpPost]
        public IActionResult AddStaffForm(String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return RedirectToAction("AddTrader", "Admin");
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return RedirectToAction("AddTrader", "Admin");
            }
            
            Models.User.Add_User(new Models.User(-1, FirstName, LastName, Email, 2, PasswordHash: Password, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now));

            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpPost]
        public IActionResult AddAdminForm(String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return RedirectToAction("AddTrader", "Admin");
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return RedirectToAction("AddTrader", "Admin");
            }
            
            Models.User.Add_User(new Models.User(-1, FirstName, LastName, Email, 0, PasswordHash: Password, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now));

            return RedirectToAction("Dashboard", "Admin");
        }
    }
}
