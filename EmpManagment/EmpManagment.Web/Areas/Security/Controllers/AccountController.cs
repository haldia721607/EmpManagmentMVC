using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using EmpManagment.Bol.IdentityEntities;
using EmpManagment.Bol.ViewModels.Security.ViewModels;
using EmpManagment.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.Security.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ILogger<AccountController> _logger;
        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ILogger<AccountController> logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
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

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (user != null && !user.EmailConfirmed && (await UserManager.CheckPasswordAsync(user, model.Password)))
                    {
                        ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                        return View(model);
                    }
                    // The last boolean parameter lockoutOnFailure indicates if the account
                    // should be locked on failed logon attempt. On every failed logon
                    // attempt AccessFailedCount column value in AspNetUsers table is
                    // incremented by 1. When the AccessFailedCount reaches the configured
                    // MaxFailedAccessAttempts which in our case is 5, the account will be
                    // locked and LockoutEnd column is populated. After the account is
                    // lockedout, even if we provide the correct username and password,
                    // PasswordSignInAsync() method returns Lockedout result and the login
                    // will not be allowed for the duration the account is locked.
                    var result = await SignInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, shouldLockout: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("AccountLocked");
                        //case SignInStatus.RequiresVerification:
                        //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        //case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                            return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email not exist");
                    return View(model);
                }

            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { Area = "Security", ReturnUrl = returnUrl }, protocol: Request.Url.Scheme));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };
            //Check any remote error from google
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }
            //Get External login info
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("Login", loginViewModel);
            }

            // Get the email claim from external login provider (Google, Facebook etc)
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            EmployeeUser user = null;
            if (email != null)
            {
                // Find the user
                user = await UserManager.FindByEmailAsync(email);
                // If email is not confirmed, display login view with validation error
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }
            //Signin with external registerd login provider
            var signInResult = await SignInManager.ExternalSignInAsync(info, isPersistent: false);
            switch (signInResult)
            {
                case SignInStatus.Success:
                    if (Convert.ToBoolean(SignInStatus.Success))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        if (email != null)
                        {
                            if (user == null)
                            {
                                user = new EmployeeUser
                                {
                                    UserName = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault(),
                                    Email = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()
                                };
                                //Create new in AspNetUsers Table
                                var result = await UserManager.CreateAsync(user);
                                if (result.Succeeded)
                                {
                                    //Insert external loging provider user in AspNetUserLogins Table
                                    await UserManager.AddLoginAsync(user.Id, info.Login);
                                    //Get user details by email id
                                    var getUserByEmail = await UserManager.FindByEmailAsync(identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault());
                                    //Generate Email Confirmation Token
                                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                    var confirmationLInk = Url.Action("ConfirmEmail", "Account", new { area = "Security", userId = getUserByEmail.Id, token = token }, protocol: Request.Url.Scheme);
                                    string Ref = "NewAccountConfirmation";
                                    int isSendSuccess = CommanFunction.SendConfirmationLink(confirmationLInk, getUserByEmail.UserName, getUserByEmail.Email, Ref);
                                    if (isSendSuccess == 1)
                                    {
                                        if (Request.IsAuthenticated && User.IsInRole("Admin"))
                                        {
                                            return RedirectToAction("Dashboard", "Dashboard", new { Area = "User" });
                                        }
                                        ViewBag.Success = "Registration successful";
                                        ViewBag.Message = "Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you";
                                        return View("RegistrationSuccessful");
                                    }
                                }
                            }
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                        ViewBag.ErrorTitle = $"Email claim not received from: {info.Login}";
                        ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";
                        return View("Error");
                    }
                case SignInStatus.LockedOut:
                    return View("AccountLocked");
                //case SignInStatus.RequiresVerification:
                //    break;
                //case SignInStatus.Failure:
                //    break;
                default:
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View("Login");
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            var countryList = accountBs.CounteryList().ToList();
            if (countryList != null)
            {
                foreach (var country in countryList)
                {
                    registerViewModel.Countries.Add(new SelectListItem { Text = country.CountryName, Value = country.CountryId.ToString() });
                }
            }
            return View(registerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new EmployeeUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        CountryId = model.countryId,
                        StateId = model.stateId,
                        CityId = model.cityId
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var getUserId = await UserManager.FindByEmailAsync(user.Email);
                        var token = await UserManager.GenerateEmailConfirmationTokenAsync(getUserId.Id);
                        var confirmationLInk = Url.Action("ConfirmEmail", "Account", new { area = "Security", userId = user.Id, token = token },protocol: Request.Url.Scheme);
                        string Ref = "NewAccountConfirmation";
                        int isSendSuccess = CommanFunction.SendConfirmationLink(confirmationLInk, model.Name, model.Email, Ref);
                        if (isSendSuccess == 1)
                        {
                            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                            {
                                return RedirectToAction("Dashboard", "Dashboard", new { Area = "User" });
                            }
                            ViewBag.Success = "Registration successful";
                            ViewBag.Message = "Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you";
                            return View("RegistrationSuccessful");
                        }
                    }
                    // If there are any errors, add them to the ModelState object
                    // which will be displayed by the validation summary tag helper
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item);
                    }
                }
                var countryList = accountBs.CounteryList().ToList();
                if (countryList != null)
                {
                    foreach (var country in countryList)
                    {
                        model.Countries.Add(new SelectListItem { Text = country.CountryName, Value = country.CountryId.ToString() });
                    }
                }
                //if (model.countryId > 0)
                //{
                //    var states = accountBs.StateList(model.countryId);
                //    if (states != null)
                //    {
                //        foreach (var item in states)
                //        {
                //            model.States.Add(new SelectListItem
                //            {
                //                Text = item.StateName,
                //                Value = Convert.ToString(item.StateId)
                //            });
                //        }
                //    }
                //}
                //if (model.stateId > 0)
                //{
                //    var cities = accountBs.CityList(model.stateId);
                //    if (cities != null)
                //    {
                //        foreach (var item in cities)
                //        {
                //            model.Cities.Add(new SelectListItem
                //            {
                //                Text = item.CityName,
                //                Value = Convert.ToString(item.CityId)
                //            });
                //        }
                //    }
                //}
                return View(model);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        string errorMessage = validationError.ErrorMessage;

                        //Or log your error message here
                    }
                }
                throw;
            }
        }
        [HttpGet]
        public JsonResult GetStatelist(int countryId)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            var states = accountBs.StateList(countryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    registerViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId)
                    });
                }
            }
            return Json(new { data = registerViewModel }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCitylist(int stateId)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            var cities = accountBs.CityList(stateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    registerViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
            }
            return Json(new { data = registerViewModel }, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<ActionResult> IsEmailInUse(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json($"Email {email} is already in use.", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null && token == null)
            {
                return RedirectToAction("Register", "Account", new { area = "Security" });
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await UserManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // Generate the reset password token
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { area = "Security", email = model.Email, token = token }, protocol: Request.Url.Scheme);
                    string Ref = "PasswordResetLink";
                    int isSendSuccess = CommanFunction.SendConfirmationLink(passwordResetLink, user.UserName, model.Email, Ref);
                    if (isSendSuccess == 1)
                    {
                        return View("ForgotPasswordConfirmation");
                    }
                    //Before you can Login, please reset your password, by clicking on the reset password link we have emailed you on your email-{model.Email} id.
                    // Log the password reset link
                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // reset the user password
                    var result = await UserManager.ResetPasswordAsync(user.Id, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        // Upon successful password reset and if the account is lockedout, set
                        // the account lockout end date to current UTC date time, so the user
                        // can login with the new password
                        if (await UserManager.IsLockedOutAsync(user.Id))
                        {
                            await UserManager.SetLockoutEndDateAsync(user.Id, DateTimeOffset.UtcNow);
                        }
                        ViewBag.Success = "Reset password successful";
                        ViewBag.Message = $"You have successfully reset your passowd.Please loging with email-{model.Email} with your new password.";
                        return View("PasswordResetSuccessful");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                ViewBag.Success = "Reset password successful";
                ViewBag.Message = $"You have successfully reset your passowd.Please loging with registered email id with your new password.";
                return View("PasswordResetSuccessful");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            return RedirectToAction("Index", "Home",new { area=""});
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Dashboard", new { Area = "User" });
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}