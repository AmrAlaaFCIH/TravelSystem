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
        public IActionResult Index(string SignUpError,string LoginError, string Agency, string Destination, string Date)
        {
            ViewBag.Posts = GetAllApprovedPosts(Agency, Destination, Date);
            ViewBag.SignUpError = SignUpError;
            ViewBag.LoginError = LoginError;
            ViewBag.Agency = Agency;
            ViewBag.Destination = Destination;
            ViewBag.Date = Date;
            return View();
        }

        private List<TripPost> GetAllApprovedPosts(string Agency, string Destination, string Date)
        {
            return context.TripPosts
                .Where(e => e.Accepted == true)
                .Where(e => e.AgencyName.Contains(Agency != null ?Agency:""))
                .Where(e => e.Destination.Contains(Destination != null ? Destination : ""))
                .Where(e => e.Date.Contains(Date != null ? Date : ""))
                .Include(e => e.Owner)
                .Include(e => e.Likedby)
                .Include(e => e.Dislikedby)
                .ToList();
        }

       
        public async Task<IActionResult> likePost(string userId, Guid postId)
        {
           await data.LikeTripPost(userId, postId);

            return RedirectToAction("Index");
        }

    }
}
