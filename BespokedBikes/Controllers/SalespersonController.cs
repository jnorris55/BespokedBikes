using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class SalespersonController : Controller
    {

        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET: Salesperson
        public ActionResult Index()
        {
            return View(_db.Salespersons.ToList());
        }

        // GET: Salesperson/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Salesperson/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Salesperson/Create
        [HttpPost]
        public ActionResult Create(Salesperson newPerson)
        {
            var nextId = _db.Salespersons.Max(x => x.Id);
            newPerson.Id = nextId+1;
            try
            {
                if (ModelState.IsValid)
                {
                    //Check if Sales person already exists
                    var existingSalesperson = (from p in _db.Salespersons where p.First_Name == newPerson.First_Name && p.Last_Name == newPerson.Last_Name select p).Count();
                    if (existingSalesperson > 0)
                    {
                        ModelState.AddModelError("", "This Salesperson Already Exists");
                        return View();
                    }
                    else
                    {
                        _db.Salespersons.Add(newPerson);
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

        // GET: Salesperson/Edit/5
        public ActionResult Edit(int id)
        {
            var personToEdit = (from m in _db.Salespersons

                                 where m.Id == id

                                 select m).First();

            return View(personToEdit);
        }

        // POST: Salesperson/Edit/5
        [HttpPost]
        public ActionResult Edit(Salesperson personToEdit)
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

        // GET: Salesperson/Delete/5
        public ActionResult Delete(int id)
        {
            var personToDelete = (from m in _db.Salespersons

                                   where m.Id == id

                                   select m).First();
            return View(personToDelete);
        }

        // POST: Salesperson/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var personToDelete = (from m in _db.Salespersons

                                      where m.Id == id

                                      select m).First();

                _db.Salespersons.Remove(personToDelete);
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
