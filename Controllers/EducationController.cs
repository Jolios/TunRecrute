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
    public class EducationController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;
        private readonly IToastNotification toastNotification;

        public EducationController(TunrecruteContext db, SignInManager<User> signInManager,IToastNotification toastNotification)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.toastNotification = toastNotification;
        }

        [HttpGet]
        [Authorize(Roles="Candidate")]
        public IActionResult Edit(int id)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            Education model = db.Educations.FirstOrDefault(e => e.Id == id);
            if (model == null) return NotFound();
            return PartialView("_Edit",model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Edit(Education model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            if (ModelState.IsValid)
            {
                Education entity = db.Educations.FirstOrDefault(e => e.Id == model.Id);
                entity.InstitutionName = model.InstitutionName;
                entity.StartYear = model.StartYear;
                entity.EndYear = model.EndYear;
                entity.DiplomaTitle = model.DiplomaTitle;
                entity.Description = model.Description;
                db.Update<Education>(entity);
                await db.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Education experience info edited !", new NotyOptions
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
            Education model = new Education();

            return PartialView("_Create", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Create(Education model)
        {
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                model.UserId = currentUser.Id;
                await db.AddAsync<Education>(model);
                await db.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Education experience added !", new NotyOptions
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
            Education education = db.Educations.FirstOrDefault(e => e.Id == id);
            if (education == null) return NotFound();
            db.Remove<Education>(education);
            await db.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Education experience deleted !", new NotyOptions
            {
                Theme = "metroui",
                Timeout = 1500,
                Layout = "topCenter"
            });
            return RedirectToAction("EditResume", "Profile");
        }
    }
}
