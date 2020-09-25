using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class SalesController : Controller
    {

        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET: Sales
        public ActionResult Index(DateTime? salesBeginDate = null, DateTime? salesEndDate = null)
        {
            var salesList = _db.Sales.ToList();

            if (salesBeginDate != null && salesEndDate != null){
                salesList = (from p in salesList where p.Sales_Date > salesBeginDate && p.Sales_Date < salesEndDate select p).ToList();
            }            

            List < SalesDetailView > salesDetailList = new List<SalesDetailView>();
            foreach (Sale sold in salesList)
            {
                SalesDetailView salesDetail = new SalesDetailView();
                salesDetail.Product = sold.Product1;
                salesDetail.Customer = sold.Customer1;
                salesDetail.Salesperson = sold.Salesperson1;
                salesDetail.Sales_Date = sold.Sales_Date;

                //Check for discount on this product for the date of sale
                var discount = (from p in _db.Discounts where p.Product == sold.Product where p.Begin_Date < sold.Sales_Date && p.End_Date > sold.Sales_Date select p).FirstOrDefault();
                if (discount != null)
                {
                   decimal actualSalePrice = sold.Product1.Sale_Price * (1 - discount.Discount_Percentage) ?? 0;
                   salesDetail.Sale_Price = actualSalePrice;
                   salesDetail.Comission = actualSalePrice * sold.Product1.Comission ?? 0;
                }else
                {
                    salesDetail.Sale_Price = sold.Product1.Sale_Price ?? 0;
                    salesDetail.Comission = sold.Product1.Sale_Price * sold.Product1.Comission ?? 0;
                }
                salesDetailList.Add(salesDetail);
            }

            //return View(_db.Sales.ToList());
            return View(salesDetailList);
        }

        // GET: Sales/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.Product = new SelectList(_db.Products, "id", "Name");

            var salespeople = _db.Salespersons
            .Select(s => new
            {
                Text = s.First_Name + " " + s.Last_Name,
                Value = s.Id
            })
            .ToList();
            ViewBag.Salesperson = new SelectList(salespeople, "Value", "Text");

            var customers = _db.Customers
            .Select(c => new
            {
               Text = c.First_Name + " " + c.Last_Name,
               Value = c.Id
            })
            .ToList();
            ViewBag.Customer = new SelectList(customers, "Value", "Text");

            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        public ActionResult Create(Sale newSale)
        {
            var nextId = _db.Sales.Max(x => x.Id);
            newSale.Id = nextId+1;
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Sales.Add(newSale);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sales/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sales/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
