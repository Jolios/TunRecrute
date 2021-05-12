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
    public class SocialMediaController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;

        public SocialMediaController(TunrecruteContext db, SignInManager<User> signInManager)
        {
            this.db = db;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddOrEdit(int id=0)
        {
            if(id == 0) return PartialView("_AddOrEdit");
            else
            {
                SocialMedia model = db.SocialMedias.FirstOrDefault(s => s.Id == id);
                if (model == null) return NotFound();
                return PartialView("_AddOrEdit", model);

            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrEdit(SocialMedia model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            bool update = true;
            SocialMedia entity = null;
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                if (model.Id == 0)
                {
                    entity = new SocialMedia();
                    entity.UserId = currentUser.Id;
                    update = false;
                }else entity = db.SocialMedias.FirstOrDefault(s => s.Id == currentUser.SocialMedia.Id);
                entity.Facebook = model.Facebook;
                entity.Behance = model.Behance;
                entity.Dribbble = model.Dribbble;
                entity.Github = model.Github;
                entity.Linkedin = model.Linkedin;
                entity.Twitter = model.Twitter;
                entity.Youtube = model.Youtube;
                entity.Instagram = model.Instagram;
                entity.Pintrest = model.Pintrest;
                if (update)
                {
                    db.Update<SocialMedia>(entity);
                }
                else
                {
                    await db.AddAsync<SocialMedia>(entity);
                }
                await db.SaveChangesAsync();

                return RedirectToAction("EditResume","Profile");
            }
            return PartialView("_AddOrEdit", model);
        }
    }
}
