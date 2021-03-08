using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.User.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: User/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}