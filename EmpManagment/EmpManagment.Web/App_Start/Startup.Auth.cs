using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using EmpManagment.Bol.IdentityEntities;
using EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels;
using EmpManagment.Dal.DbContextClass;
using EmpManagment.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Authorization;
using Microsoft.Owin.Security.Authorization.Infrastructure;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace EmpManagment.Web
{
	public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(EmployeeDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                LoginPath = new PathString("/Security/Account/Login"),
                LogoutPath = new PathString("/Security/Account/Logout"),
                
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, EmployeeUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Enable the External Sign In Cookie.
            //app.SetDefaultSignInAsAuthenticationType("External");

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
            //    ExpireTimeSpan = TimeSpan.FromMinutes(5),
            //    LoginPath = new PathString("/Security/Account/Login"),
            //    LogoutPath = new PathString("/Security/Account/Logout"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, EmployeeUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: ConfigurationManager.AppSettings["FacebookAppId"],
               appSecret: ConfigurationManager.AppSettings["FacebookScreteKey"]);

            var google = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "765893291210-pk4bkl0r4k964oaiac46nq0715oklp8a.apps.googleusercontent.com",
                ClientSecret = "GU-lyts-SUIPsUHKI4E5EOYg",
                Provider = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        if (!String.IsNullOrEmpty(context.RefreshToken))
                        {
                            context.Identity.AddClaim(new Claim("RefreshToken", context.RefreshToken));
                        }
                        return Task.FromResult<object>(null);
                    }
                }
                //Provider = new GoogleOAuth2AuthenticationProvider()
            };
            google.Scope.Add("email");
            app.UseGoogleAuthentication(google);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "Delete Role"));
                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "Edit Role"));//policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                //If Handlers failuer then do not invoke next Handlers
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "Create Role"));
                //options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin", "Super Admin"));//policy.AddRequirements(new AdministrationRolePolicy()));
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("SuperAdmin", policy => policy.RequireRole("Super Admin"));
                options.AddPolicy("UserRolePolicy", policy => policy.RequireRole("User", "Manager"));
            });
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}