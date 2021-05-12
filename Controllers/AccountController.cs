using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;
using Tunrecrute.ViewModels;

namespace Tunrecrute.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult ChooseRole()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterEmployer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployer(EmployerRegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User { 
                    UserName = model.Email,
                    Email = model.Email,
                    CompanyName = model.CompanyName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    IsActive = 1};

                if (! await roleManager.RoleExistsAsync("Employer"))
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = "Employer"
                    };
                    var resultRole = await roleManager.CreateAsync(identityRole);
                    if (!resultRole.Succeeded)
                    {
                        foreach (IdentityError error in resultRole.Errors) ModelState.AddModelError("", error.Description);
                    }
                }
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employer");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Advertisement");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterCandidate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCandidate(CandidateRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    IsActive  = 1
                };

                if (!await roleManager.RoleExistsAsync("Candidate"))
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = "Candidate"
                    };
                    var resultRole = await roleManager.CreateAsync(identityRole);
                    if (!resultRole.Succeeded)
                    {
                        foreach (IdentityError error in resultRole.Errors) ModelState.AddModelError("", error.Description);
                    }
                }
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Candidate");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Advertisement");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,false,false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    else return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
    
}
