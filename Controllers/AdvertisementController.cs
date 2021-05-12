using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using Tunrecrute.ViewModels;
using TunRecrute.Data;

namespace Tunrecrute.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;

        public AdvertisementController(TunrecruteContext db,SignInManager<User> signInManager)
        {
            this.db = db;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index(int pageNumber = 1,int pageSize = 10)
        {
            AdvertisementsIndexViewModel model = new AdvertisementsIndexViewModel();
            model.Searched = false;
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var query = db.Advertisements.OrderBy(x => x.Posted)
                .Select(p => p)
                .Skip(ExcludeRecords)
                .Take(pageSize);
            model.Ads = new PagedResult<Advertisement>();
            model.Ads.Data = await query.ToListAsync();
            model.Ads.TotalItems = await db.Advertisements.CountAsync();
            model.Ads.PageNumber = pageNumber;
            model.Ads.PageSize = pageSize;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Find(AdvertisementsIndexViewModel model)
        {
            model.Title??= "";
            model.Diploma??= "";
            model.WorkAddress??= "";
            model.Contract??= "";
            model.Experience??= "";
            model.Searched = true;
            var query = db.Advertisements.Where(x => x.Title.Contains(model.Title)
            && x.Field.Contains(model.Field)
            && x.WorkAddress.Contains(model.WorkAddress)
            && x.Diploma.Contains(model.Diploma)
            && x.Contract.Contains(model.Contract)
            && x.Experiance.Contains(model.Experience))
                .OrderBy(x => x.Posted);
            model.Ads = new PagedResult<Advertisement>();
            model.Ads.Data = await query.ToListAsync();
            model.Ads.TotalItems = await db.Advertisements.CountAsync();
            //model.Ads.PageNumber = pageNumber;
            //model.Ads.PageSize = pageSize;
            return View("Index",model);
        }
        [HttpGet]
        [Authorize(Roles ="Employer")]
        public IActionResult Create()
        {
            ViewBag.Fields = new List<SelectListItem>
            {
            new SelectListItem { Value = "Banking and Finance", Text = "Banking and Finance" },
            new SelectListItem { Value = "Education", Text = "Education"  },
            new SelectListItem { Value = "Consulting", Text = "Consulting"  },
            new SelectListItem { Value = "Marketing and Advertising", Text = "Marketing and Advertising" },
            new SelectListItem { Value = "Technology", Text = "Technology"  },
            };
            ViewBag.Contracts = new List<SelectListItem>
            {
            new SelectListItem { Value = "CDI", Text = "CDI" },
            new SelectListItem { Value = "CDD", Text = "CDD"  },
            new SelectListItem { Value = "Full-Time", Text = "Full-Time"  },
            new SelectListItem { Value = "Independent/Freelance", Text = "Independent/Freelance" },
            new SelectListItem { Value = "Internship", Text = "Internship"  },
            };
            ViewBag.Diplomas = new List<SelectListItem>
            {
            new SelectListItem { Value = "No Diploma", Text = "No Diploma" },
            new SelectListItem { Value = "Bachelor", Text = "Bachelor"  },
            new SelectListItem { Value = "PhD", Text = "PhD"  },
            new SelectListItem { Value = "Master", Text = "Master"  },
            };
            ViewBag.Experiences = new List<SelectListItem>
            {
            new SelectListItem { Value = "No Experience", Text = "No Experience" },
            new SelectListItem { Value = "1-2", Text = "1-2" },
            new SelectListItem { Value = "3-4", Text = "3-4"  },
            new SelectListItem { Value = "5-6", Text = "5-6"  },
            new SelectListItem { Value = "7-8", Text = "7-8"  },
            new SelectListItem { Value = "9+", Text = "9+"  },
            };
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> Create(Advertisement ad)
        {
            if (ModelState.IsValid)
            {
                ad.User = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
                ad.Posted = DateTime.Now;
                db.Add<Advertisement>(ad);
                db.SaveChanges();
                return RedirectToAction("Show",new { id = ad.Id });
            }
            return View(ad);
            
        }
        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return NotFound();
            var ad = await db.Advertisements.SingleOrDefaultAsync(a => a.Id == id.Value);
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue) return NotFound();
            var ad = await db.Advertisements.SingleOrDefaultAsync(a => a.Id == id);
            var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
            if (ad == null) return NotFound();
            if (!ad.UserId.Equals(user.Id)) return RedirectToAction("Index");
            ViewBag.Fields = new List<SelectListItem>
            {
            new SelectListItem { Value = "Banking and Finance", Text = "Banking and Finance" },
            new SelectListItem { Value = "Education", Text = "Education"  },
            new SelectListItem { Value = "Consulting", Text = "Consulting"  },
            new SelectListItem { Value = "Marketing and Advertising", Text = "Marketing and Advertising" },
            new SelectListItem { Value = "Technology", Text = "Technology"  },
            };
            ViewBag.Contracts = new List<SelectListItem>
            {
            new SelectListItem { Value = "CDI", Text = "CDI" },
            new SelectListItem { Value = "CDD", Text = "CDD"  },
            new SelectListItem { Value = "Full-Time", Text = "Full-Time"  },
            new SelectListItem { Value = "Independent/Freelance", Text = "Independent/Freelance" },
            new SelectListItem { Value = "Internship", Text = "Internship"  },
            };
            ViewBag.Diplomas = new List<SelectListItem>
            {
            new SelectListItem { Value = "No Diploma", Text = "No Diploma" },
            new SelectListItem { Value = "Bachelor", Text = "Bachelor"  },
            new SelectListItem { Value = "PhD", Text = "PhD"  },
            new SelectListItem { Value = "Master", Text = "Master"  },
            };
            ViewBag.Experiences = new List<SelectListItem>
            {
            new SelectListItem { Value = "No Experience", Text = "No Experience" },
            new SelectListItem { Value = "1-2", Text = "1-2" },
            new SelectListItem { Value = "3-4", Text = "3-4"  },
            new SelectListItem { Value = "5-6", Text = "5-6"  },
            new SelectListItem { Value = "7-8", Text = "7-8"  },
            new SelectListItem { Value = "9+", Text = "9+"  },
            };
            return View(ad);
        }
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> MyJobs(int pageNumber = 1, int pageSize = 10)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            PagedResult<Advertisement> model = new PagedResult<Advertisement>();
            model.Data = currentUser.Advertisements.ToList();
            model.TotalItems = currentUser.Advertisements.Count;
            model.PageNumber = pageNumber;
            model.PageSize = pageSize;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="Employer")]
        public IActionResult Edit(Advertisement ad)
        {
            if (ModelState.IsValid)
            {
                db.Update(ad);
                db.SaveChanges();
                return RedirectToAction("Show", ad.Id);
            }
            return View(ad);
            
        }
        [HttpPost]
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ad = await db.Advertisements.SingleOrDefaultAsync(a => a.Id == id);
            var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
            if (ad == null) return NotFound();
            if (!ad.UserId.Equals(user.Id)) return RedirectToAction("Index");
            db.Advertisements.Remove(ad);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
