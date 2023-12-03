using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THW.Controllers
{
    public class ModuleController : Controller
    {
        MenusDAO menusDAO = new MenusDAO();
        ///////////////////////////////////////////////////////////////////
        //GET: MainMenu
        public ActionResult MainMenu()
        {
            return View(menusDAO.getListByParentId(0));
        }
    }
}