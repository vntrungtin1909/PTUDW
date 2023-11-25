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
using UDW.Library;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace THW.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        private SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thogn bao that bai
                TempData["message"] = new XMessage("danger", "tạo mới thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "tạo mới thất bại");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");

            return View();
        }

        // POST: Admin/Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu lu tu dong: slug, createat/by, updateAt/By, order
                // xu li tu dong cho: CreateAt
                suppliers.CreateAt = DateTime.Now;

                suppliers.UpdateAt = DateTime.Now;

                suppliers.CreateBy = Convert.ToInt32(Session["UserId"]);

                suppliers.UpdateBy = Convert.ToInt32(Session["UserId"]);


                // xu li tu dong cho: order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                // xu li tu dong: slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + suppliers.Id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Img = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/Supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //chen mau tin vao DB
                suppliersDAO.Insert(suppliers);
                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "tạo mới thành công");
                return RedirectToAction("Index");

            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");

            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "thong bao ton tai nha cung cap");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "khogn ton tai nha cung cap");

                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");

            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                // xu li tu dong cho: CreateAt
                suppliers.CreateAt = DateTime.Now;

                suppliers.UpdateAt = DateTime.Now;



                // xu li tu dong cho: order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                // xu li tu dong: slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/Supplier/";
                if (img.ContentLength != 0 && suppliers.Img != null)
                {
                    //xoa anh cu
                    string delPath = Path.Combine(Server.MapPath(PathDir), suppliers.Img);
                    System.IO.File.Delete(delPath);
                }

                    //XU LI CHO PHAN XOA HINH ANH
                if (img.ContentLength != 0) 
                { 
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + suppliers.Id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Img = imgName;
                        //upload hinh

                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                

                suppliersDAO.Update(suppliers);
                TempData["message"] = new XMessage("success", "cap nhat nha cung cap thanh cong");

                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");

            return View(suppliers);
        }

        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "khogn ton tai nha cung cap");

                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "khogn ton tai nha cung cap");

                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            //xoa nau tin ra khoi DB
            suppliersDAO.Delete(suppliers);
            TempData["message"] = new XMessage("success", "xoa nha cung cap thanh cong");

            return RedirectToAction("Index");

        }

        //phat sinh 1 so action: status, trash, deltrash, undo tu categoriesController

        /// ////////////////////////////////////////////////////////////////////////    
        ///
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "cap nhat trang thai that bai");
                return RedirectToAction("Index");
            }
            //truy van id
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "cap nhat trang thai that bai");
                return RedirectToAction("Index");
            }

            //chuyen doi trang thai cua status tu 1<->2
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;

            //cap nhat gia tri updateAt
            suppliers.UpdateAt = DateTime.Now;

            //cap nhat database
            suppliersDAO.Update(suppliers);

            //cap nhat trang thai thanh cong
            TempData["message"] = new XMessage("success", "cap nhat trang thai thanh cong");
            return RedirectToAction("Index");
        }

        public ActionResult MoveTrash(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //truy van id
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            //chuyen doi trang thai cua status bang 0
            suppliers.Status = 0;

            //cap nhat gia tri updateAt
            suppliers.UpdateAt = DateTime.Now;

            //cap nhat database
            suppliersDAO.Update(suppliers);

            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "mẫu tin chuyển vào thùng rác thành công");
            //TempData["message"] = ("cap nhat trang thai thanh cong");
            return RedirectToAction("Index");
        }

        // TRASH
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));//chi hien thi cac dong status bang = 0
        }


        // RECOVER
        public ActionResult Recover(int? id)
        {
            //Categories categories = new Categories();
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //truy van id
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            //chuyen doi trang thai cua status bang 0
            suppliers.Status = 2;

            //cap nhat gia tri updateAt
            suppliers.UpdateAt = DateTime.Now;

            //cap nhat database
            suppliersDAO.Update(suppliers);

            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "phục hồi mẫu tin thành công");

            return RedirectToAction("Trash"); //action trong category
        }


    }
}
