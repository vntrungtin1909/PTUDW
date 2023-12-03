using MyClass.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using THW.Common;
using THW.Models;

namespace THW.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UsersDAO u = new UsersDAO();
                var result = u.Login(model.Email, model.Password);
                if (result == 1)
                {
                    var user = u.getItem(model.Email);
                    var session = new UserLogin();
                    session.Email = user.Email;
                    session.UserId = user.Id;
                    session.UserName = user.UserName;
                    session.FullName = user.FullName;
                    Session.Add(Vari.USER_SESSION, session);
                    return RedirectToAction("Index", "Category");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");

                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Email không tồn tại");

                }
            }
            return View("Index");
        }
    }
}