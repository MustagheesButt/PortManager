using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;

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

            //var senderEmail = new MailAddress("bcsf16m538@pucit.edu.pk", "Sheiikh993");
            //var receiverEmail = new MailAddress(receiver, "Receiver");
            //var password = "Your Email Password here";
            //var sub = subject;
            //var body = message;

            Models.User user = Models.User.GetUserByEmail(email);

            string from = "bcsf16m538@pucit.edu.pk";
            string to = email;

            MailMessage message = new MailMessage(from , to);
            //message.From = from;
            //message.To = email;
            message.Subject = "Password Recovery";
            message.Body = $"Your Password is {user.password_hash} ";

            SmtpClient smpt = new SmtpClient();
            smpt.Host = "smtp@gmail.com";
            smpt.Port = 587;
            //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smpt.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("bcsf16m538@pucit.edu.pk", "Sheikh993");
            smpt.UseDefaultCredentials = true;
            smpt.Credentials = nc;

            try
            {
                smpt.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
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
