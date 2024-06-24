using MVCproject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCproject.Controllers
{
    public class ProductsController : Controller
    {
        AppDbContext db = new AppDbContext();

        #region showing all products for admin 

        public ActionResult Index()
        {
            var query = db.Products.ToList();
            return View(query);
        }

        #endregion


        #region products add for admin

        public ActionResult Create()
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");
            return View();
        }



        [HttpPost]
        public ActionResult Create(Product p, HttpPostedFileBase Image)
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");


            if (ModelState.IsValid)
            {


                Product pro = new Product();
                pro.Name = p.Name;
                pro.Description = p.Description;
                pro.Unit = p.Unit;
                pro.Image = Image.FileName.ToString();
                pro.CategoryId = p.CategoryId;
                pro.Popularity = 1;

                //image upload
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));

                db.Products.Add(pro);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Product Not Upload";
            }
            return View();
        }


        #endregion


        #region edit products

        public ActionResult Edit(int id)
        {

            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");

            var query = db.Products.SingleOrDefault(m => m.ProductId == id);

            return View(query);
        }


        //        [HttpPost]
        //        public ActionResult Edit(Product p, HttpPostedFileBase Image)
        //{
        //    List<Category> list = db.Categories.ToList();
        //    ViewBag.CatList = new SelectList(list, "CategoryId", "Name", p.CategoryId);

        //    if (ModelState.IsValid)
        //    {
        //        Product pro = db.Products.Find(p.ProductId);
        //        if (pro == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        pro.Name = p.Name;
        //        pro.Description = p.Description;
        //        pro.Unit = p.Unit;
        //        pro.CategoryId = p.CategoryId;

        //        if (Image != null && Image.ContentLength > 0)
        //        {
        //            var folder = Server.MapPath("~/Uploads/");
        //            var path = Path.Combine(folder, Image.FileName.ToString());

        //            // Delete the old image if necessary
        //            if (System.IO.File.Exists(Path.Combine(folder, pro.Image)))
        //            {
        //                System.IO.File.Delete(Path.Combine(folder, pro.Image));
        //            }

        //            Image.SaveAs(path);
        //            pro.Image = Image.FileName.ToString();
        //            }

        //            db.Entry(pro).State = EntityState.Modified;
        //            db.SaveChanges();

        //            return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //            TempData["msg"] = "Product Not Updated";
        //            }

        //            //return View(p);
        //            return RedirectToAction("Index");
        //            }


        [HttpPost]
        public ActionResult Edit(Product p, HttpPostedFileBase Image)
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name", p.CategoryId);

            if (ModelState.IsValid)
            {
                Product pro = db.Products.Find(p.ProductId);
                if (pro == null)
                {
                    return HttpNotFound();
                }

                pro.Name = p.Name;
                pro.Description = p.Description;
                pro.Unit = p.Unit;
                pro.CategoryId = p.CategoryId;
                //pro.Discount = p.Discount;
                //pro.FinalRate = (decimal)(p.Unit - (p.Unit * p.Discount / 100));

                if (Image != null && Image.ContentLength > 0)
                {
                    var folder = Server.MapPath("~/Uploads/");
                    var path = Path.Combine(folder, Image.FileName.ToString());

                    // Delete the old image if necessary
                    if (System.IO.File.Exists(Path.Combine(folder, pro.Image)))
                    {
                        System.IO.File.Delete(Path.Combine(folder, pro.Image));
                    }

                    Image.SaveAs(path);
                    pro.Image = Image.FileName.ToString();
                }

                db.Entry(pro).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Product Not Updated";
            }

            //return View(p);
            return RedirectToAction("Index");
        }


        public ActionResult AllProduct(string searchQuery, string sortOrder, int? page, int? categoryId)
        {
            if (TempData["cart"] != null)
            {
                int x = 0;
                List<Cart> cartItems = TempData["cart"] as List<Cart>;
                foreach (var item in cartItems)
                {
                    x += item.bill;
                }
                TempData["total"] = x;
                TempData["item_count"] = cartItems.Count();
            }
            TempData.Keep();

            var products = db.Products.AsQueryable();

            // Apply search query filter if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery));
            }

            // Apply category filter if provided
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.SelectedCategory = categoryId;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.NameAscSortParam = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceAscSortParam = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";

            // Apply sorting based on sortBy and sortOrder parameters
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(c => c.Name);
                    break;
                case "name_asc":
                    products = products.OrderBy(c => c.Name);
                    break;
                case "price":
                    products = products.OrderBy(c => c.Unit);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(c => c.Unit);
                    break;
                case "price_asc":
                    products = products.OrderBy(c => c.Unit);
                    break;
                default:
                    products = products.OrderBy(c => c.ProductId);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            return View(pagedProducts);
        }


        public ActionResult ProductsByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return RedirectToAction("AllProduct");
            }

            return RedirectToAction("AllProduct", new { categoryId });
        }

        #endregion


        #region delete product 

        public ActionResult Delete(int id)
        {
            var query = db.Products.SingleOrDefault(m => m.ProductId == id);
            db.Products.Remove(query);

            db.SaveChanges();


            return RedirectToAction("Index");

        }

        #endregion



    }
}
