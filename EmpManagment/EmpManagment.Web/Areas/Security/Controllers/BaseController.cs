using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.Security.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        public AccountBs accountBs = new AccountBs();
        public BaseController()
        {
            accountBs = new AccountBs();
        }
    }
}