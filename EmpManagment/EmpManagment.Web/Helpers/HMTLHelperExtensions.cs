using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Authorization;
using Microsoft.Owin.Security.Authorization.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpManagmentInMVC.Helpers
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }
    }
    public static class Policy
    {
        public static bool IsClaime(this HtmlHelper html,string sPolicy)
        {
            var user = (ClaimsIdentity)HttpContext.Current.User.Identity;
            if ("CreateRolePolicy" == sPolicy)
            {
                var claimValue = user.Claims.Where(x => x.Type == "Create Role" && x.Value == "Create Role").Select(y => y.Value).FirstOrDefault();
                if (claimValue!=null)
                {
                    if (claimValue.ToString() == "Create Role")
                    {
                        return true;
                    }
                }
                return false;
            }
            if ("EditRolePolicy" == sPolicy)
            {
                var claimValue = user.Claims.Where(x => x.Type == "Edit Role" && x.Value == "Edit Role").Select(y => y.Value).FirstOrDefault();

                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    if (claimValue != null)
                    {
                        if (claimValue.ToString() == "Edit Role")
                        {
                            return true;
                        }
                    }
                }
                if(HttpContext.Current.User.IsInRole("Super Admin"))
                {
                    return true;
                }
                return false;
            }
            if ("DeleteRolePolicy" == sPolicy)
            {
                var claimValue = user.Claims.Where(x => x.Type == "Delete Role" && x.Value == "Delete Role").Select(y => y.Value).FirstOrDefault();

                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    if (claimValue != null)
                    {
                        if (claimValue.ToString() == "Delete Role")
                        {
                            return true;
                        }
                    }
                }
                if (HttpContext.Current.User.IsInRole("Super Admin"))
                {
                    return true;
                }
            }
            return false;
        }
    }
    //public class PolicyCheck
    //{
    //    private static IAuthorizationService _IAuthorizationService;
    //    public PolicyCheck(IAuthorizationService authorizationService)
    //    {
    //        _IAuthorizationService = authorizationService;
    //    }
    //    public static bool ss = false;
    //    public static async Task<bool> IsPolicy(string policys)
    //    {
    //        if ((ClaimsPrincipal)HttpContext.Current.User != null)
    //        {
    //            ss = await _IAuthorizationService.AuthorizeAsync((ClaimsPrincipal)HttpContext.Current.User, policys);
    //            return ss;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}
}