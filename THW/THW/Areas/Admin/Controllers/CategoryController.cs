using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using System.Xml.Linq;
using UDW.Library;

namespace THW.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        //// GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                // xu li tu dong cho: CreateAt
                categories.CreateAt = DateTime.Now;

                categories.UpdateAt = DateTime.Now;

                // xu li tu dong: parentid
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }

                // xu li tu dong cho: order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }

                // xu li tu dong: slug
                categories.Slug = XString.Str_SLug(categories.Name);

                //chen them dong bo cho DB
                categoriesDAO.Insert(categories);
                return RedirectToAction("Index");
            }

            return View(categories);
        }

        //// GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Update(categories);
                return RedirectToAction("Index");
               
            }
            return View(categories);
        }

        //// GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
           
            return RedirectToAction("Index");
        }


        public ActionResult Status(int? id)
        {
            //Categories categories = new Categories();
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "cap nhat trang thai that bai");
                return RedirectToAction("Index");
            }
            //truy van id
            Categories categories = categoriesDAO.getRow(id);

            //chuyen doi trang thai cua status tu 1<->2
            categories.Status = (categories.Status == 1) ? 2 : 1;

            //cap nhat gia tri updateAt
            categories.UpdateAt = DateTime.Now;

            //cap nhat database
            categoriesDAO.Update(categories);

            //cap nhat trang thai thanh cong
            TempData["message"] = TempData["message"] = new XMessage("success", "cap nhat trang thai thanh cong");
            return RedirectToAction("Index");
        }

    }
}
