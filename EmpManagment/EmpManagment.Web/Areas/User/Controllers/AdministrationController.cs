using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using EmpManagment.Bol.IdentityEntities;
using EmpManagment.Bol.ViewModels.Security.ViewModels;
using EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels;
using EmpManagment.Web.Models;
using EmpManagment.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Authorization;
using Microsoft.Owin.Security.Authorization.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace EmpManagment.Web.Areas.User.Controllers
{
    [AuthorizeUser(Roles = "Admin,Super Admin")]
    public class AdministrationController : BaseController
    {
        // GET: User/Administration
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ILogger<AdministrationController> _logger;
        private IAuthorizationService _IAuthorizationService;
        public AdministrationController()
        {

        }
        public AdministrationController(ApplicationRoleManager roleManagers, ApplicationUserManager userManager, ApplicationSignInManager signInManager, ILogger<AdministrationController> logger, IAuthorizationService authorizationService)
        {
            RoleManager = (ApplicationRoleManager)roleManagers;
            UserManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
            _IAuthorizationService = authorizationService;
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
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [HttpGet]
        [AuthorizeUserPolicy(Policy = "CreateRolePolicy")]
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserPolicy(Policy = "CreateRolePolicy")]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await RoleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ListRoles()
        {
            var roles = RoleManager.Roles;
            string superAdminId = roles.Where(x => x.Name == "Super Admin").Select(c=>c.Id).FirstOrDefault(); ;
            if (superAdminId != null)
            {
                return View(roles.Where(x => x.Id != superAdminId));
            }
            return View(roles);
        }
        [HttpGet]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditRole(string id)
        {
            //Find role by id
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={id} cannot be found";
                return View();
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            // Retrieve all the Users
            foreach (var user in UserManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await RoleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                // Update the Role using UpdateAsync
                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration", new { area = "User" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        [AuthorizeUserPolicy(Policy = "DeleteRolePolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                // Wrap the code in a try/catch block
                try
                {
                    //throw new Exception("Test Exception");

                    var result = await RoleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    return View("ListRoles");
                }
                // If the exception is DbUpdateException, we know we are not able to
                // delete the role as there are users in the role being deleted
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file. We discussed logging to a file
                    // using Nlog in Part 63 of ASP.NET Core tutorial
                    _logger.LogError($"Exception Occured : {ex}");
                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("Error");
                }
            }
        }
        [HttpGet]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in UserManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditUsersInRole(List<UserRoleViewModel> models, string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < models.Count; i++)
            {
                var user = await UserManager.FindByIdAsync(models[i].UserId);
                IdentityResult result = null;
                if (models[i].IsSelected && !(await UserManager.IsInRoleAsync(user.Id, role.Name)))
                {
                    result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
                else if (!models[i].IsSelected && await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    result = await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (models.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
        [HttpGet]
        public async Task<ActionResult> Users()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault(); // will give the user's userId
            var user = await UserManager.FindByIdAsync(userId);
            if (await UserManager.IsInRoleAsync(user.Id, "Super Admin"))
            {
                return View(UserManager.Users);
            }
            else
            {
                string adminId = null;
                foreach (var item in UserManager.Users)
                {
                    if (await UserManager.IsInRoleAsync(item.Id, "Super Admin"))
                    {
                        adminId = item.Id;
                    }
                }
                if (adminId != null)
                {
                    return View(UserManager.Users.Where(u => u.Id != userId && u.Id != adminId));
                }
                //Login user not see own data also super admin data
                return View(UserManager.Users.Where(u => u.Id != userId));
            }
        }
        [HttpGet]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditUser(string userId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            //Find user by id
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            BindMasterData(editUserViewModel, user);
            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await UserManager.GetClaimsAsync(user.Id);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await UserManager.GetRolesAsync(user.Id);
            editUserViewModel.Id = user.Id;
            editUserViewModel.Email = user.Email;
            editUserViewModel.UserName = user.UserName;
            editUserViewModel.City = user.City;
            editUserViewModel.CountryId = user.CountryId;
            editUserViewModel.StateId = user.StateId;
            editUserViewModel.CityId = user.CityId;
            editUserViewModel.Claims = userClaims.Select(c => c.Value).ToList();
            editUserViewModel.Roles = userRoles;
            return View(editUserViewModel);
        }
        private void BindMasterData(EditUserViewModel editUserViewModel, ApplicationUser user)
        {
            //Bind Country
            var countryList = accountBs.CounteryList().ToList();
            if (countryList != null)
            {
                foreach (var country in countryList)
                {
                    editUserViewModel.Countries.Add(new SelectListItem { Text = country.CountryName, Value = country.CountryId.ToString() });
                }
            }
            //Bind State
            var states = accountBs.StateList(user.CountryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    editUserViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId)
                    });
                }
            }
            //Bind City
            var cities = accountBs.CityList(user.StateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    editUserViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
            }
        }
        [HttpGet]
        public JsonResult GetStatelist(int countryId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            var states = accountBs.StateList(countryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    editUserViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId)
                    });
                }
                return Json(editUserViewModel);
            }
            return Json(null);
        }
        [HttpGet]
        public JsonResult GetCitylist(int stateId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            var cities = accountBs.CityList(stateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    editUserViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
                return Json(editUserViewModel);
            }
            return Json(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserPolicy(Policy = "EditRolePolicy")]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.CountryId = model.CountryId;
                user.StateId = model.StateId;
                user.CityId = model.CityId;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                BindMasterData(model, user);
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserPolicy(Policy = "DeleteRolePolicy")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await UserManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View("Users");
            }
        }
        [HttpGet]
        public async Task<ActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in RoleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await UserManager.GetRolesAsync(user.Id);
            if (roles.Count > 0)
            {
                foreach (var item in roles)
                {
                    var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot remove user existing roles");
                        ViewBag.userId = userId;
                        return View(model);
                    }
                }

            }

            foreach (var item in model)
            {
                if (item.IsSelected == true)
                {
                    var addRole = await UserManager.AddToRoleAsync(user.Id, item.RoleName);
                    if (!addRole.Succeeded)
                    {
                        ModelState.AddModelError("", $"Cannot add {item.RoleName} roles to user");
                        return View(model);
                    }
                }
            }
            return RedirectToAction("EditUser", new { userId = userId });
        }
        [HttpGet]
        public async Task<ActionResult> ManageUserClaims(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await UserManager.GetClaimsAsync(user.Id);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await UserManager.GetClaimsAsync(user.Id);
            if (claims.Count > 0)
            {
                foreach (var item in model.Cliams)
                {
                    IdentityResult result = await UserManager.RemoveClaimAsync(user.Id, new Claim(item.ClaimType, item.ClaimType));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot remove user existing claims");
                        return View(model);
                    }
                }
            }
            // Add all the claims that are selected on the UI
            if (model.UserId != null)
            {
                foreach (var item in model.Cliams)
                {
                    if (item.IsSelected == true)
                    {
                        IdentityResult result2 = await UserManager.AddClaimAsync(user.Id, new Claim(item.ClaimType, item.ClaimType));
                        if (!result2.Succeeded)
                        {
                            ModelState.AddModelError("", "Cannot add selected claims to user");
                            return View(model);
                        }
                    }
                }
            }
            return RedirectToAction("EditUser", new { Id = model.UserId });

        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}