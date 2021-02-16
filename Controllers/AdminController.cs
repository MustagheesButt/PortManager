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

        [HttpGet("Admin/EditUser/{user_id}")]
        public IActionResult EditUser(int user_id)
        {
            return View("EditUser" , user_id);
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

        [HttpPost]
        public IActionResult EditUserForm(int user_id , String FirstName, String LastName, String Email, String Password, String ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["errors"] = "Password and Confirm Password don't match";
                return View("EditUser", user_id);
            }

            // check if email exists
            if (Models.User.GetUserByEmail(Email) != null)
            {
                TempData["errors"] = $"{Email} is already registered with us.";
                return View("EditUser", user_id);
            }

            //Models.User user = Models.User.GetUserByEmail(Email);
            
            Models.User.Edit_User(user_id ,FirstName , LastName , Email , Password);

            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpGet("Admin/DeleteUser/{user_id}")]
        public IActionResult DeleteUser(int user_id)
        {
           
            PortManager.Models.User.Delete(user_id);

            return RedirectToAction("Dashboard", "Admin");
        }
    }
}
