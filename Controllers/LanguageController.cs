using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using Tunrecrute.ViewModels;
using TunRecrute.Data;

namespace Tunrecrute.Controllers
{
    public class LanguageController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;

        public LanguageController(TunrecruteContext db, SignInManager<User> signInManager)
        {
            this.db = db;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public IActionResult Edit(int id)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            Language entity = db.Languages.FirstOrDefault(e => e.Id == id);
            if (entity == null) return NotFound();
            LanguageViewModel model = new LanguageViewModel
            {
                list = new List<SelectListItem>
                {
                    new SelectListItem{Text="Native",Value="Native"},
                    new SelectListItem{Text="Advanced",Value="Advanced"},
                    new SelectListItem{Text="Intermediate",Value="Intermediate"},
                    new SelectListItem{Text="Novice",Value="Novice"}
                }
            };
            model.language = new Language
            {
                Id = entity.Id,
                LanguageName = entity.LanguageName,
                ProfeciencyLevel = entity.ProfeciencyLevel
            };
            return PartialView("_Edit", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Edit(LanguageViewModel model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            if (ModelState.IsValid)
            {
                Language entity = db.Languages.FirstOrDefault(e => e.Id == model.language.Id);
                entity.LanguageName = model.language.LanguageName;
                entity.ProfeciencyLevel = model.language.ProfeciencyLevel;
                db.Update<Language>(entity);
                await db.SaveChangesAsync();
            }
            return PartialView("_Edit", model);
        }
        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public IActionResult Create()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            LanguageViewModel model = new LanguageViewModel
            {
                language = new Language(),
                list = new List<SelectListItem>
                {
                    new SelectListItem{Text="Native",Value="Native"},
                    new SelectListItem{Text="Advanced",Value="Advanced"},
                    new SelectListItem{Text="Intermediate",Value="Intermediate"},
                    new SelectListItem{Text="Novice",Value="Novice"}
                }
            };

            return PartialView("_Create", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Create(LanguageViewModel model)
        {
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                Language entity = new Language();
                entity.UserId = currentUser.Id;
                entity.ProfeciencyLevel = model.language.ProfeciencyLevel;
                entity.LanguageName = model.language.LanguageName;
                await db.AddAsync<Language>(entity);
                await db.SaveChangesAsync();
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
            Language language = db.Languages.FirstOrDefault(e => e.Id == id);
            if (language == null) return NotFound();
            db.Remove<Language>(language);
            await db.SaveChangesAsync();
            return RedirectToAction("EditResume", "Profile");
        }
    }
}
