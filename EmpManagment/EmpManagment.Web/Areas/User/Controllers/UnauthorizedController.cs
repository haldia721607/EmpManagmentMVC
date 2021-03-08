using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.User.Controllers
{
    [AllowAnonymous]
    public class UnauthorizedController : Controller
    {
        // GET: User/Unauthorized
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}