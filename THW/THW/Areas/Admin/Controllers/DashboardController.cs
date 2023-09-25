using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THW.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/DashboardAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}