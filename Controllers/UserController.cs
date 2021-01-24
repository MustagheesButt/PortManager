using System;
using Microsoft.AspNetCore.Mvc;
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
            String hash = PortManager.Models.User.hash(Password);

            if (user.PasswordHash == hash)
            {
                // successfull login. set session and redirect
                HttpContext.Session.SetInt32("user_id", user.id);

                if (user.UserType == 0)
                    return RedirectToAction("Dashboard", "Admin");
                else if (user.UserType == 1)
                    return RedirectToAction("Dashboard", "Trader");
                else
                    return RedirectToAction("Dashboard", "PortStaff");
            }

            ViewBag.Errors = "Passowrd does not match";
            return Redirect("Login");
        }
    }
}
