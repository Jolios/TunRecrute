using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using Tunrecrute.ViewModels;
using TunRecrute.Data;

namespace Tunrecrute.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TunrecruteContext db;

        public HomeController(ILogger<HomeController> logger,TunrecruteContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Advertisement> ads = db.Advertisements.OrderByDescending(a =>a.Posted).Take<Advertisement>(5);
            int numA = db.Advertisements.Count();
            int numU = db.Users.Count();
            ViewBag.jobsCount = numA;
            ViewBag.usersCount = numU;
            return View(ads);
        }

        [HttpPost]
        public  IActionResult Find(string title,string address)
        {
            if(String.IsNullOrEmpty(title) && String.IsNullOrEmpty(address))
            {
                return RedirectToAction("Index");
            }
            AdvertisementsIndexViewModel model = new AdvertisementsIndexViewModel
            {
                Title = title,
                WorkAddress = address
            };
            return RedirectToAction("Find","Advertisement",model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
