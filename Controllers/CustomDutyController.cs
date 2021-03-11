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
    public class CustomDutyController : Controller
    {
        [Route("/CustomDuties")]
        public IActionResult Index()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            //int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            ViewData["custom_duties"] = CustomDuty.GetAll();

            return View("Index");
        }

        public IActionResult Add()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Decimal amount, string currency, DateTime DueDate, int ship_id)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            CustomDuty cd = new CustomDuty(-1, ship_id, amount, currency, DueDate, CreatedAt: DateTime.Now, UpdatedAt: DateTime.Now);
            CustomDuty.Add(cd);

            return RedirectToAction("Index", "CustomDuty");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            CustomDuty cd = CustomDuty.GetOne(id);
            ViewData["custom_duty"] = cd;

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, Decimal amount, string currency, DateTime DueDate, DateTime PaidAt)
        {
            int trader_id = (int)HttpContext.Session.GetInt32("user_id");
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            // TODO verify custom_duty belongs to the current_user/trader

            CustomDuty cd = CustomDuty.GetOne(id);
            cd.Amount = amount;
            cd.Currency = currency;
            cd.DueDate = DueDate;
            cd.PaidAt = PaidAt;
            cd.UpdatedAt = DateTime.Now;

            CustomDuty.Update(cd);

            return RedirectToAction("Index", "CustomDuty");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            CustomDuty.DeleteById(id);
            return RedirectToAction("Index", "CustomDuty");
        }

        [HttpGet]
        public IActionResult MarkPaid(int id)
        {
            CustomDuty cd = CustomDuty.GetOne(id);
            cd.Status = "Paid";
            cd.PaidAt = DateTime.Now;
            CustomDuty.Update(cd);

            return RedirectToAction("Index", "CustomDuty");
        }
    }
}
