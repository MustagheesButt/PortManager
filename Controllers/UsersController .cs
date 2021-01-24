using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortManager.Models;
using Microsoft.AspNetCore.Http;

namespace PortManager.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult Login(String Email, String Password)
        {
            PortManager.Models.User user = PortManager.Models.User.GetUserByEmail(Email);
            
            if (user == null)
            {
                ViewBag.Errors = "Email not found";
                return Redirect("Login");
            }

            // hash input password
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);

            if (user.PasswordHash == hash)
            {
                // set session
                
            }
            else
            {
                ViewBag.Errors = "Passowrd does not match";
                return Redirect("Login");
            }

            if (user.UserType == 0)
                return RedirectToAction("Dashboard", "Admin");
            else if (user.UserType == 1)
                return RedirectToAction("Dashboard", "Trader");
            else
                return RedirectToAction("Dashboard", "PortStaff");
        }
    }
}
