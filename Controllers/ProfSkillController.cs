using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using TunRecrute.Data;

namespace Tunrecrute.Controllers
{
    public class ProfSkillController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;

        public ProfSkillController(TunrecruteContext db, SignInManager<User> signInManager)
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
            ProfSkill model = db.ProfSkills.FirstOrDefault(e => e.Id == id);
            if (model == null) return NotFound();
            return PartialView("_Edit", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Edit(ProfSkill model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            if (ModelState.IsValid)
            {
                ProfSkill entity = db.ProfSkills.FirstOrDefault(e => e.Id == model.Id);
                entity.Name = model.Name;
                entity.Percent= model.Percent;
                db.Update<ProfSkill>(entity);
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
            ProfSkill model = new ProfSkill();

            return PartialView("_Create", model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Create(ProfSkill model)
        {
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                model.UserId = currentUser.Id;
                await db.AddAsync<ProfSkill>(model);
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
            ProfSkill education = db.ProfSkills.FirstOrDefault(e => e.Id == id);
            if (education == null) return NotFound();
            db.Remove<ProfSkill>(education);
            await db.SaveChangesAsync();
            return RedirectToAction("EditResume", "Profile");
        }
    }
}
