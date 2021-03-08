using EmpManagment.Bll.SecurityBs;
using EmpManagment.Bll.UserBs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.User.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public AccountBs accountBs = new AccountBs();
        public SqlComplaientBs sqlComplaientBs = new SqlComplaientBs();
        public BaseController()
        {
            accountBs = new AccountBs();
            sqlComplaientBs = new SqlComplaientBs();
        }
    }
}