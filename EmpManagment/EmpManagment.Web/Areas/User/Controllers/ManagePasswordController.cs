using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.User.Controllers
{
    [Authorize]
    public class ManagePasswordController : Controller
    {
        // GET: User/ManagePassword
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public ManagePasswordController()
        {

        }
        public ManagePasswordController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var userHasPassword = await UserManager.HasPasswordAsync(user.Id);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await UserManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await SignInManager.SignInAsync(user,isPersistent:false,rememberBrowser:false);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AddPassword()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var userHasPassword = await UserManager.HasPasswordAsync(user.Id);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View();
                }
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return View("AddPasswordConfirmation");
            }
            return View(model);
        }
    }
}