using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using Microsoft.Data.SqlClient;


namespace PortManager.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Forget_Password()
        {
            return Redirect("/Forget");
        }
        
        [HttpPost]
        public IActionResult ForgetForm(string email)
        {
            Models.User user = Models.User.GetUserByEmail(email);

            string from = "YOUR_EMAIL_HERE";
            string to = email;

            MailMessage message = new MailMessage(from , to);
            //message.From = from;
            //message.To = email;
            message.Subject = "Password Recovery";
            string pass = user.PasswordHash.ToString();
            message.Body = $"Your Password is {user.PasswordHash} ";

            SmtpClient smpt = new SmtpClient();
            smpt.Host = "smtp.gmail.com";
            smpt.Port = 587;
            //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smpt.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("YOUR_EMAIL_HERE", "YOUR_PASSWORD_HERE");
            smpt.UseDefaultCredentials = false;
            smpt.Credentials = nc;

            try
            {
                smpt.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return Redirect("/Login");
        }

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
            //String hash = Models.User.hash(Password);

            if (user.PasswordHash.Trim() == Password)
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

        public IActionResult Edit(int id)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            ViewData["user"] = Models.User.GetOne(id);
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Update(int id, String FirstName, String LastName, String Email)
        {
            Models.User.Update(id, FirstName, LastName, Email);

            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Delete(int id)
        {
            Models.User.Delete(id);

            return RedirectToAction("Dashboard", "Admin");
        }
    }
}
