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
    public class StaticController : Controller
    {
        private readonly ILogger<StaticController> _logger;

        public StaticController(ILogger<StaticController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [HttpGet("{page}")]
        public IActionResult Index(string page)
        {
            _logger.Log(LogLevel.Information, $"page={page}");
            return View(page);
        }

        [Route("Trends")]
        [HttpGet]
        public IActionResult Trends()
        {
            var x = Helper.Protect(HttpContext.Session);
            if (x != null) return x;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
