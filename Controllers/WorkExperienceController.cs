using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using TunRecrute.Data;

namespace Tunrecrute.Controllers
{
    public class WorkExperienceController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;
        private readonly IToastNotification toastNotification;

        public WorkExperienceController(TunrecruteContext db, SignInManager<User> signInManager,IToastNotification toastNotification)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.toastNotification = toastNotification;
        }

        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public IActionResult Edit(int id)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            WorkExperience model = db.WorkExperiences.FirstOrDefault(e => e.Id == id);
            if (model == null) return NotFound();
            return PartialView("_Edit", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Edit(WorkExperience model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            if (ModelState.IsValid)
            {
                WorkExperience entity = db.WorkExperiences.FirstOrDefault(e => e.Id == model.Id);
                entity.CompanyName = model.CompanyName;
                entity.StartYear = model.StartYear;
                entity.EndYear = model.EndYear;
                entity.Title = model.Title;
                entity.Description = model.Description;
                db.Update<WorkExperience>(entity);
                await db.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Work Experience Updated !", new NotyOptions
                {
                    Theme = "metroui",
                    Timeout = 1500,
                    Layout = "topCenter"
                });
            }
            return PartialView("_Edit", model);
        }
        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public IActionResult Create()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            WorkExperience model = new WorkExperience();

            return PartialView("_Create", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Create(WorkExperience model)
        {
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                model.UserId = currentUser.Id;
                await db.AddAsync<WorkExperience>(model);
                await db.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Work Experience Added !", new NotyOptions
                {
                    Theme = "metroui",
                    Timeout = 1500,
                    Layout = "topCenter"
                });
            }
            return PartialView("_Create", model);
        }
        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public IActionResult Delete(int id)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            ViewBag.Id = id;
            return PartialView("_Delete");
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> DeleteOnPost(int id)
        {
            WorkExperience education = db.WorkExperiences.FirstOrDefault(e => e.Id == id);
            if (education == null) return NotFound();
            db.Remove<WorkExperience>(education);
            await db.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Work Experience Deleted !", new NotyOptions
            {
                Theme = "metroui",
                Timeout = 1500,
                Layout = "topCenter"
            });
            return RedirectToAction("EditResume", "Profile");
        }
    }
}
