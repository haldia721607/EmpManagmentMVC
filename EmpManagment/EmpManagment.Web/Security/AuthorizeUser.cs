using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Authorization.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmpManagment.Web.Security
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {

                //if not logged, it will work as normal Authorize and redirect to the Login
                if (httpContext.User.IsInRole("Admin") || httpContext.User.IsInRole("Super Admin"))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //logged and wihout the role to access it - redirect to the custom controller action
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "User", controller = "Unauthorized", action = "AccessDenied" }));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
    public class AuthorizeUserPolicy : ResourceAuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                bool IsPolicy = false;
                var user = (ClaimsIdentity)HttpContext.Current.User.Identity;
                if ("CreateRolePolicy" == Policy)
                {
                    var claimValue = user.Claims.Where(x => x.Type == "Create Role" && x.Value == "Create Role").Select(y => y.Value).FirstOrDefault();
                    if (claimValue!=null)
                    {
                        if (claimValue.ToString() == "Create Role")
                        {
                            return true;
                        }
                    }
                }
                if ("EditRolePolicy" == Policy)
                {
                    var claimValue = user.Claims.Where(x => x.Type == "Edit Role" && x.Value == "Edit Role").Select(y => y.Value).FirstOrDefault();
                    if (httpContext.User.IsInRole("Admin"))
                    {
                        if (claimValue != null)
                        {
                            if (claimValue.ToString() == "Edit Role")
                            {
                                IsPolicy = true;
                            }
                        }
                    }
                    if (httpContext.User.IsInRole("Super Admin"))
                    {
                        return true;
                    }
                    var adminIdBeingEdited = httpContext.Request.RequestContext.RouteData.Values.Where(x => x.Key == "id").Select(y => y.Value).FirstOrDefault();
                    string loggedInAdminId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    if (IsPolicy == true && Convert.ToString(adminIdBeingEdited).ToLower() != loggedInAdminId.ToLower())
                    {
                        return true;
                    }
                    return false;
                }
                if ("DeleteRolePolicy" == Policy)
                {
                    var claimValue = user.Claims.Where(x => x.Type == "Delete Role" && x.Value == "Delete Role").Select(y => y.Value).FirstOrDefault();
                    if (httpContext.User.IsInRole("Admin"))
                    {
                        if (claimValue!=null)
                        {
                            if (claimValue.ToString() == "Delete Role")
                            {
                                IsPolicy = true;
                            }
                        }
                    }
                    if (httpContext.User.IsInRole("Super Admin"))
                    {
                        return true;
                    }
                    var adminIdBeingEdited = httpContext.Request.RequestContext.RouteData.Values.Where(x => x.Key == "id").Select(y => y.Value).FirstOrDefault();
                    string loggedInAdminId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    if (IsPolicy == true && Convert.ToString(adminIdBeingEdited).ToLower() != loggedInAdminId.ToLower())
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //logged and wihout the role to access it - redirect to the custom controller action
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "User", controller = "Administration", action = "AccessDenied" }));
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}