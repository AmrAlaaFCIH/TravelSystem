using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Controller;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IDataControl data;
        private readonly AppDBContext context;

        public HomeController(ILogger<HomeController> logger,IDataControl data,AppDBContext context)
        {
            this.logger = logger;
            this.data = data;
            this.context = context;
        }

        [Route("")]
        public IActionResult Index(string SignUpError,string LoginError)
        {
            ViewBag.Posts = GetAllApprovedPosts();
            ViewBag.SignUpError = SignUpError;
            ViewBag.LoginError = LoginError;
            return View();
        }

        private List<TripPost> GetAllApprovedPosts()
        {
            return context.TripPosts
                .Where(e => e.Accepted == true)
                .Include(e => e.Owner)
                .Include(e => e.Likedby)
                .Include(e => e.Dislikedby)
                .ToList();
        }
    }
}
