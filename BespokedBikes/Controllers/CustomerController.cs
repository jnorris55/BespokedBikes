using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class CustomerController : Controller
    {
        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET: Customer
        public ActionResult Index()
        {
            return View(_db.Customers.ToList());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer newCustomer)
        {
            var nextId = _db.Customers.Max(x => x.Id);
            newCustomer.Id = nextId + 1;
            try
            {
                if (ModelState.IsValid)
                {
                    //Check if Sales person already exists
                    var existingCustomer = (from p in _db.Customers where p.First_Name == newCustomer.First_Name && p.Last_Name == newCustomer.Last_Name select p).Count();
                    if (existingCustomer > 0)
                    {
                        ModelState.AddModelError("", "This Customer Already Exists");
                        return View();
                    }
                    else
                    {
                        _db.Customers.Add(newCustomer);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            var personToEdit = (from m in _db.Customers

                                where m.Id == id

                                select m).First();

            return View(personToEdit);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(Customer personToEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _db.Entry(personToEdit).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                {
                    return View(personToEdit);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            var personToDelete = (from m in _db.Customers

                                 where m.Id == id

                                 select m).First();
            return View(personToDelete);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var personToDelete = (from m in _db.Customers

                                      where m.Id == id

                                      select m).First();

                _db.Customers.Remove(personToDelete);
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
