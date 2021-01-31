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
            Models.User user = Models.User.GetUserByEmail(Email);

            if (user == null)
            {
                TempData["errors"] = "Email not found";
                return Redirect("/Login");
            }

            // hash input password
            String hash = Models.User.hash(Password);

            if (user.PasswordHash.Trim().Equals(hash))
            {
                // successfull login. set session and redirect
                HttpContext.Session.SetInt32("user_id", user.id);

                if (user.Type == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else if (user.Type == "Trader")
                    return RedirectToAction("Dashboard", "Trader");
                else
                    return RedirectToAction("Dashboard", "PortStaff");
            }

            TempData["errors"] = "Password does not match";
            return Redirect("/Login");
        }
    }
}
