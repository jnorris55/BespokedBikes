using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class ReportController : Controller
    {
        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET: Report
        public ActionResult Index(string quarterNames = null)
        {
            ReportQuarter reportInfo = new ReportQuarter();
            reportInfo.id = 1;
            reportInfo.quarterNames = new List<string>();
            reportInfo.reportData = new List<CommissionReport>();

            DateTime beginDate = new DateTime();
            DateTime endDate = new DateTime();
            if (quarterNames == null)
            {
                // Default to current quarter
                var currentDate = DateTime.Now;                
                if (currentDate.Month < 4)
                {
                    beginDate = new DateTime(currentDate.Year, 1, 1);
                    endDate = new DateTime(currentDate.Year, 3, 31);
                }
                else if (currentDate.Month < 7)
                {
                    beginDate = new DateTime(currentDate.Year, 4, 1);
                    endDate = new DateTime(currentDate.Year, 6, 30);
                }else if (currentDate.Month < 10)
                {
                    beginDate = new DateTime(currentDate.Year, 6, 1);
                    endDate = new DateTime(currentDate.Year, 9, 30);
                }else
                {
                    beginDate = new DateTime(currentDate.Year, 10, 1);
                    endDate = new DateTime(currentDate.Year, 12, 31);
                }
            }
            else
            {
                // Get dates for selected quarter
                int quarter = Int16.Parse(quarterNames.Substring(1, 2));
                int year = Int16.Parse(quarterNames.Substring(2, 5));
                switch (quarter)
                {
                    case 1:
                        beginDate = new DateTime(year, 1, 1);
                        endDate = new DateTime(year, 3, 31);
                        break;
                    case 2:
                        beginDate = new DateTime(year, 4, 1);
                        endDate = new DateTime(year, 6, 30);
                        break;
                    case 3:
                        beginDate = new DateTime(year, 7, 1);
                        endDate = new DateTime(year, 9, 30);
                        break;
                    case 4:
                        beginDate = new DateTime(year, 10, 1);
                        endDate = new DateTime(year, 12, 31);
                        break;
                }                
            }

            List<CommissionReport> reportData = new List<CommissionReport>();
            var activeSalespeople = (from p in _db.Sales where p.Sales_Date >= beginDate && p.Sales_Date <= endDate select p.Salesperson).Distinct().ToList();

            foreach (int agentId in activeSalespeople)
            {
                CommissionReport salespersonReport = new CommissionReport();

                Salesperson salesAgent = (from p in _db.Salespersons where p.Id == agentId select p).FirstOrDefault();
                salespersonReport.Name = salesAgent.First_Name + " " + salesAgent.Last_Name;
                List<Sale> agentSales = (from p in _db.Sales where p.Sales_Date >= beginDate && p.Sales_Date <= endDate && p.Salesperson == agentId select p).ToList();
                salespersonReport.bikesSold = agentSales.Count;

                List<SalesDetailView> agentSalesDetail = new List<SalesDetailView>();
                foreach (Sale sold in agentSales)
                {
                    SalesDetailView salesDetail = new SalesDetailView();
                    //Check for discount on this product for the date of sale
                    var discount = (from p in _db.Discounts where p.Product == sold.Product where p.Begin_Date < sold.Sales_Date && p.End_Date > sold.Sales_Date select p).FirstOrDefault();
                    if (discount != null)
                    {
                        decimal actualSalePrice = sold.Product1.Sale_Price * (1 - discount.Discount_Percentage) ?? 0;
                        salesDetail.Sale_Price = actualSalePrice;
                        salesDetail.Comission = actualSalePrice * sold.Product1.Comission ?? 0;
                    }
                    else
                    {
                        salesDetail.Sale_Price = sold.Product1.Sale_Price ?? 0;
                        salesDetail.Comission = sold.Product1.Sale_Price * sold.Product1.Comission ?? 0;
                    }
                    agentSalesDetail.Add(salesDetail);
                }
                salespersonReport.totalSales = agentSalesDetail.Sum(x => x.Sale_Price);
                salespersonReport.totalComissions = agentSalesDetail.Sum(x => x.Comission);

                reportData.Add(salespersonReport);
            }
            reportInfo.reportData = reportData;

            DateTime firstSale = (from p in _db.Sales orderby p.Sales_Date ascending select p.Sales_Date).First();
            DateTime currentYear = DateTime.Now;                        
            for (var i = firstSale.Year; i <= currentYear.Year; i++)
            {
                int startMonth = 1;
                if (i == firstSale.Year)
                {
                    startMonth = firstSale.Month;
                }
                int endMonth = 12;
                if (i == currentYear.Year)
                {
                    endMonth = currentYear.Month;
                }
                for (var j = startMonth; j <= endMonth; j = j + 3)
                {
                    string quarterAvailable;
                    if (j < 4) { quarterAvailable = "Q1 " + i; }
                    else if (j < 7) { quarterAvailable = "Q2 " + i;  }
                    else if (j < 10) { quarterAvailable = "Q3 " + i; }
                    else { quarterAvailable = "Q4 " + i; }
                    reportInfo.quarterNames.Add(quarterAvailable);
                }
            }
            //ViewBag.Quarters = new SelectList(reportInfo.quarterNames, "Value", "Text");

            return View(reportInfo);
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
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

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
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
