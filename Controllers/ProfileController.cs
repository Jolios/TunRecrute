using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using Tunrecrute.ViewModels;
using TunRecrute.Data;

#nullable enable

namespace Tunrecrute.Controllers
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkAllFieldsAsSkipped(this ModelStateDictionary modelState,string key)
        {
            foreach (var state in modelState)
            {
                if (state.Key.Contains(key))
                {
                    state.Value.Errors.Clear();
                    state.Value.ValidationState = ModelValidationState.Skipped;
                }
            }
            return modelState;
        }
    }
    public class ProfileController : Controller
    {
        private readonly TunrecruteContext db;
        private readonly SignInManager<User> signInManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProfileController(TunrecruteContext db,SignInManager<User> signInManager,IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> EditCandidateProfile()
        {
            var currentUser = this.User;
            var user = await signInManager.UserManager.GetUserAsync(currentUser);
            var model = new EditCandidateProfileViewModel
            {
                Candidate = user
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> EditEmployerProfile()
        {
            User user = await signInManager.UserManager.GetUserAsync(this.User);
            EditEmployerViewModel model = new EditEmployerViewModel
            {
                Employer = user,
                social = user.SocialMedia
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> EditCandidateProfile(EditCandidateProfileViewModel model)
        {
            ModelState.MarkAllFieldsAsSkipped("Password");
            var user = await signInManager.UserManager.GetUserAsync(this.User);
            if(user == null)
            {
                return RedirectToAction("Login","Account");
            }
            if (model.ImageFile == null) model.Candidate.Picture = user.Picture;
            if (ModelState.IsValid)
            {
                try
                {
                    if(model.ImageFile != null)
                    {
                        string imageFileName = new string(Path.GetFileNameWithoutExtension(model.ImageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                        string newFileName = imageFileName + '-' + Guid.NewGuid().ToString() + '.' + Path.GetExtension(model.ImageFile.FileName);
                        string destination = Path.Combine(hostEnvironment.WebRootPath, "uploads/UserImages");
                        destination = Path.Combine(destination, newFileName);
                        using(var fileStream = new FileStream(destination, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(fileStream);
                        }
                        user.Picture = newFileName;
                    }
                    user.Firstname = model.Candidate.Firstname;
                    user.Lastname = model.Candidate.Lastname;
                    user.Email = model.Candidate.Email;
                    user.Address = model.Candidate.Address;
                    user.PhoneNumber = model.Candidate.PhoneNumber;
                    db.Update<User>(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("EditCandidateProfile");
                }
                catch (Exception  ex )
                {
                    Console.WriteLine(ex.Message);
                    
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> EditEmployerProfile(EditCandidateProfileViewModel model)
        {
            var currentUser = this.User;
            var user = await signInManager.UserManager.GetUserAsync(currentUser);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="Candidate")]
        public async Task<IActionResult> ChangePasswordCandidate(EditCandidateProfileViewModel model)
        {
            ModelState.ClearValidationState("Candidate");
            if (ModelState.IsValid)
            {
                var user = await signInManager.UserManager.GetUserAsync(this.User);
                if(user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var reuslt = await signInManager.UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!reuslt.Succeeded)
                {
                    foreach(var error in reuslt.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("EditCandidateProfile");
                }
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("EditCandidateProfile");
               
               
            }
            return View("EditCandidateProfile",model);
        }
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ChangePasswordEmployer(EditEmployerViewModel model)
        {
            ModelState.ClearValidationState("Employer");
            if (ModelState.IsValid)
            {
                var user = await signInManager.UserManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var reuslt = await signInManager.UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!reuslt.Succeeded)
                {
                    foreach (var error in reuslt.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("EditEmployerProfile");
                }
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("EditEmployerProfile");


            }
            return View("EditEmployerProfile", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Resume(string? id)
        {
            var currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            if(await signInManager.UserManager.IsInRoleAsync(currentUser,"Employer") && id != null)
            {
                var candidate = await signInManager.UserManager.FindByIdAsync(id);
                if (candidate == null) return NotFound();
                return View("ResumeForEmployer", candidate); 
            }else return View(currentUser);
        }


        [Authorize]
        public async Task<IActionResult> DownloadCV()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            User user = await signInManager.UserManager.GetUserAsync(this.User);
            string path = Path.Combine(hostEnvironment.WebRootPath, "uploads/UserUploads/") + user.CVFilename;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", user.CVFilename);
        }

        [Authorize]
        public async Task<IActionResult> DownloadCL()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            User user = await signInManager.UserManager.GetUserAsync(this.User);
            string path = Path.Combine(hostEnvironment.WebRootPath, "uploads/UserUploads") + user.CoverLetterFilename;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", user.CoverLetterFilename);
        }

        [HttpGet]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> EditResume()
        {
            User candidate = await signInManager.UserManager.GetUserAsync(this.User);
            EditResumeViewModel model = new EditResumeViewModel
            {
                Candidate = candidate
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> EditResume(EditResumeViewModel model)
        {
            ModelState.MarkAllFieldsAsSkipped("Candidate");

            User candidate = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.Candidate.Email));
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.CVFile != null)
                    {
                        string originalFileName = new string(Path.GetFileNameWithoutExtension(model.CVFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                        string newFileName = originalFileName + '-' + Guid.NewGuid().ToString() + '.' + Path.GetExtension(model.CVFile.FileName);
                        string destination = Path.Combine(hostEnvironment.WebRootPath, "uploads/UserFiles");
                        destination = Path.Combine(destination, newFileName);
                        using (var fileStream = new FileStream(destination, FileMode.Create))
                        {
                            model.CVFile.CopyTo(fileStream);
                        }
                        candidate.CVFilename = newFileName;
                    }
                    if (model.CoverLetterFile != null)
                    {
                        string originalFileName = new string(Path.GetFileNameWithoutExtension(model.CoverLetterFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                        string newFileName = originalFileName + '-' + Guid.NewGuid().ToString() + '.' + Path.GetExtension(model.CoverLetterFile.FileName);
                        string destination = Path.Combine(hostEnvironment.WebRootPath, "uploads/UserFiles");
                        destination = Path.Combine(destination, newFileName);
                        using (var fileStream = new FileStream(destination, FileMode.Create))
                        {
                            model.CoverLetterFile.CopyTo(fileStream);
                        }
                        candidate.CoverLetterFilename = newFileName;
                    }
                    db.Update<User>(candidate);
                    await db.SaveChangesAsync();
                    return RedirectToAction("EditResume");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            return View(model);
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet]
        public async Task<IActionResult> AboutUser()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            AboutUserViewModel model = new AboutUserViewModel
            {
                AboutMe = currentUser.AboutMe
            };
            return PartialView("_AboutUser", model);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> AboutUser(AboutUserViewModel model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            
            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == currentUser.Id);
                currentUser.AboutMe = model.AboutMe;
                db.Update<User>(currentUser);
                await db.SaveChangesAsync();
            }
            return PartialView("_AboutUser", model);
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet]
        public async Task<IActionResult> UserDetails()
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            UserDetailsViewModel model = new UserDetailsViewModel() {
                Email = currentUser.Email,
                Firstname = currentUser.Firstname,
                Lastname = currentUser.Lastname,
                Address = currentUser.Address,
                Nationality = currentUser.Nationality,
                Birthdate = currentUser.Birthdate,
                Sex = currentUser.Sex,
                selectList = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Male", Value = "Male"},
                    new SelectListItem {Text = "Female", Value = "Female"}

                }
                
            };
            return PartialView("_UserDetails", model);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> UserDetails(UserDetailsViewModel model)
        {
            var header = this.Request.Headers["sec-fetch-site"];
            if (header == "none") return RedirectToAction("Index", "Advertisement");

            if (ModelState.IsValid)
            {
                User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
                currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == currentUser.Id);
                currentUser.Firstname = model.Firstname;
                currentUser.Lastname = model.Lastname;
                currentUser.Email = model.Email;
                currentUser.Birthdate = model.Birthdate;
                currentUser.Sex = model.Sex;
                currentUser.Address = model.Address;
                db.Update<User>(currentUser);
                await db.SaveChangesAsync();
            }
            return PartialView("_UserDetails", model);
        }

        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> AppliedJobs(int pageNumber=1,int pageSize = 5)
        {
            User currentUser = await signInManager.UserManager.GetUserAsync(this.User);
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var query = db.Candidacies.Select(p => p)
                .Where(x => x.UserId == currentUser.Id && x.IsDeleted == 0)
                .Skip(ExcludeRecords)
                .Take(pageSize);
            List<Candidacy> model = new List<Candidacy>();
            model = await query.ToListAsync();
            return View(model);
        }
    }
}
