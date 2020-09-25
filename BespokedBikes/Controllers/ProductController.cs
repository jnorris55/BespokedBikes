using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class ProductController : Controller
    {

        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET: Product
        public ActionResult Index()
        {
            return View(_db.Products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product newProduct)
        {
            var nextId = _db.Products.Max(x => x.Id);
            newProduct.Id = nextId + 1;
            try
            {
                if (ModelState.IsValid)
                {
                    //Check if this product already exists
                    var existingProduct = (from p in _db.Products where p.Name == newProduct.Name select p).Count();
                    if (existingProduct > 0){
                        ModelState.AddModelError("", "This Product Already Exists");
                        return View();
                    }else
                    {
                        _db.Products.Add(newProduct);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Unable to Save Product");
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var productToEdit = (from m in _db.Products

                               where m.Id == id

                               select m).First();

            return View(productToEdit);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product productToEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _db.Entry(productToEdit).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Index");

                }else
                {
                    return View(productToEdit);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var productToDelete = (from m in _db.Products

                                 where m.Id == id

                                 select m).First();
            return View(productToDelete);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var productToDelete = (from m in _db.Products

                                       where m.Id == id

                                       select m).First();

                _db.Products.Remove(productToDelete);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
