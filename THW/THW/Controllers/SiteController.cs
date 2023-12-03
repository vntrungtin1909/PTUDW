using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
namespace THW.Controllers
{
    public class SiteController : Controller
    {
        MenusDAO menusDAO = new MenusDAO();
        ProductsDAO productsDAO = new ProductsDAO();
        // GET: Site
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult ChitietSanpham(int? id)
        {
            Products products = productsDAO.getRow(id);
            return View(products);
        }

        public ActionResult DSSanpham()
        {
            ProductsDAO productsDAO = new ProductsDAO();
            ViewBag.ProductsDAO = productsDAO;
            return View(productsDAO.getList("DSSanpham"));
        }

        public ActionResult DSLoaiSP(string name)
        {
            ProductsDAO productsDAO = new ProductsDAO();
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            List<Categories> c = categoriesDAO.getList();
            List<Products> p = productsDAO.getList();
            foreach (var item in c)
            {
                if (item.Name == name)
                {
                    ViewBag.ProductsDAO = p.Where(m => m.CatId == item.Id).ToList();
                }
            }
            return View(ViewBag.ProductsDAO);
        }
    }
}