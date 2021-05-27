using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Controller;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IDataControl data;

        public HomeController(ILogger<HomeController> logger,IDataControl data)
        {
            this.logger = logger;
            this.data = data;
        }

        public IActionResult Index()
        {
            return Json(data.GetAllUsers()[0]);
        }

    }
}
