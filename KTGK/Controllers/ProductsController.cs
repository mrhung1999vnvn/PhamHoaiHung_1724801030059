using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KTGK.Models;
using PagedList;



namespace KTGK.Controllers
{
    public class ProductsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categorys);
            var categories = from cate in db.Categorys
                             select cate;

            ViewBag.SelectedList = categories.ToList();
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categorys, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,ShortName,Note,CategoryId")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categorys, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categorys, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,ShortName,Note,CategoryId")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categorys, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [ActionName("Sort")]
        public ActionResult Index(string Sorting_Order,FormCollection collection)
        {
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";
            var products = from stu in db.Products select stu;
            switch (Sorting_Order)
            {
                case "Name":
                    products = products.OrderByDescending(stu => stu.Name);
                    break;
                case "Id":
                    products = products.OrderBy(item => item.Id);
                    break;
                
            }
            return View("Index",products.ToList());
        }

        [ActionName("Filter")]
        public ActionResult Index(string value)
        {
            var x = Request.Form[""];

            return View("Index");
        }


        //Seaching data
        [HttpPost, ActionName("Search")]
        public ActionResult Index(string Sorting_Order, string Search_Data)
        {
            var x = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingDate = Sorting_Order == "Date_Enroll" ? "Date_Description" : "Date";

            var students = from stu in db.Products select stu;
            {
                students = students.Where(stu => stu.Name.ToUpper().Contains(Search_Data.ToUpper())
                    || stu.Id.ToString().ToUpper().Contains(Search_Data.ToUpper()));
            }

            switch (Sorting_Order)
            {
                case "Id":
                    students = students.OrderByDescending(stu => stu.Id);
                    break;
                case "Name":
                    students = students.OrderBy(stu => stu.Name);
                    break;
            }

            return View(students.ToList());
        }

        //Paged List
        [ActionName("Paged")]
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var products = from stu in db.Products select stu;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                products = products.Where(stu => stu.Name.ToUpper().Contains(Search_Data.ToUpper()));
            }
            switch (Sorting_Order)
            {
                case "Name":
                    products = products.OrderByDescending(stu => stu.Name);
                    break;
            }

            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);
            return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
