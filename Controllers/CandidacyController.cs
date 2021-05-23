using cloudscribe.Pagination.Models;
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
    public class CandidacyController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;
        private readonly IToastNotification toastNotification;

        public CandidacyController(TunrecruteContext db, SignInManager<User> signInManager, IToastNotification toastNotification)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.toastNotification = toastNotification;
        }

        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> Applications(int pageNumber = 1, int pageSize = 5)
        {
        User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
           int ExcludeRecords = (pageSize * pageNumber) - pageSize;
           var query = db.Advertisements.Select(a => a)
            .Where(a => a.UserId == currentUser.Id);

           var secondQuery = query.SelectMany(a => a.Candidacies)
            .Skip(ExcludeRecords)
            .Take(pageSize);
           PagedResult<Candidacy> model  = new PagedResult<Candidacy>();
           model.Data = secondQuery.ToList();
           model.TotalItems = secondQuery.Count();
           model.PageSize = pageSize;
           model.PageNumber = pageNumber;
           return View(model);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ManagedApplications(int pageNumber = 1, int pageSize = 5)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var query = db.Advertisements.Select(a => a)
             .Where(a => a.UserId == currentUser.Id);

            var secondQuery = query.SelectMany(a => a.Candidacies);

            var thirdQuery = secondQuery.Select(c=>c)
                .Where(c=>!String.IsNullOrEmpty(c.Status))
                 .Skip(ExcludeRecords)
                .Take(pageSize); 
            PagedResult<Candidacy> model = new PagedResult<Candidacy>();
            model.Data = secondQuery.ToList();
            model.TotalItems = secondQuery.Count();
            model.PageSize = pageSize;
            model.PageNumber = pageNumber;
            return View(model);
        }
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AdCandidacies(int adId,int pageNumber = 1, int pageSize = 5)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var query = db.Candidacies.Select(a => a)
             .Where(a => a.AdvertisementId == adId)
             .Skip(ExcludeRecords)
             .Take(pageSize); 

            PagedResult<Candidacy> model = new PagedResult<Candidacy>();
            model.Data = query.ToList();
            model.TotalItems = query.Count();
            model.PageSize = pageSize;
            model.PageNumber = pageNumber;
            return View(model);
        }
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Create(Advertisement ad)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            Candidacy cd = new Candidacy();
            cd.Advertisement = ad;
            cd.Date = DateTime.Now;
            cd.User = currentUser;
            cd.IsDeleted = 0;
            db.Candidacies.Add(cd);
            await db.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Your application has been sent !", new NotyOptions
            {
                Theme = "metroui",
                Timeout = 1500,
                Layout = "topCenter"
            });
            return RedirectToAction("Show","Advertisement",new { id = ad.Id });
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ChangeStatus(int adId, string candidateId, string status)
        {
            Candidacy cd = db.Candidacies.FirstOrDefault(a => a.AdvertisementId == adId && a.UserId.Equals(candidateId));
            cd.Status = status;
            db.Candidacies.Update(cd);
            await db.SaveChangesAsync();
            return RedirectToAction("AdCandidacies",new { adId = adId });
        }
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Delete(int adId)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            Candidacy cd = db.Candidacies.FirstOrDefault(a => a.AdvertisementId == adId && a.UserId.Equals(currentUser.Id));
            cd.IsDeleted = 1;
            db.Candidacies.Update(cd);
            await db.SaveChangesAsync();
            return RedirectToAction("AppliedJobs","Profile");
        }
    }
}
