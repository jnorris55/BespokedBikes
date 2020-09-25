using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BespokedBikes.Controllers
{
    public class HomeController : Controller
    {
        private BikesDBEntities2 _db = new BikesDBEntities2();

        public ActionResult Index()
        {
            Dashboard homeDashboard = new Dashboard();
            homeDashboard.salesThisMonth = new List<CommissionReport>();
            homeDashboard.hotBike = new List<BikeSummary>();

            DateTime currentDate = DateTime.Now;
            DateTime beginDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endDate = beginDate.AddMonths(1);

            // Dashboard get current Discounts (Sales)
            homeDashboard.currentDiscounts = (from p in _db.Discounts where beginDate < currentDate && currentDate < endDate select p).ToList();

            // Dashboard get sales summary for the current month
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

                homeDashboard.salesThisMonth.Add(salespersonReport);                
            }
            homeDashboard.salesThisMonth = homeDashboard.salesThisMonth.OrderByDescending(x => x.totalSales).ToList();

            // Dashboard get bikes popular for last 3 months
            DateTime popularStartDate = currentDate.AddMonths(-3);

            var bikesSold = (from p in _db.Sales where p.Sales_Date > popularStartDate && p.Sales_Date <= currentDate select p.Product1.Name).Distinct().ToList();
            foreach (string bikeName in bikesSold)
            {
                BikeSummary bikeTrend = new BikeSummary();
                bikeTrend.bikeName = bikeName;
                bikeTrend.bikesSold = (from p in _db.Sales where p.Sales_Date > popularStartDate && p.Sales_Date <= currentDate && p.Product1.Name == bikeName select p).ToList().Count();
                homeDashboard.hotBike.Add(bikeTrend);
            }
            homeDashboard.hotBike = homeDashboard.hotBike.OrderByDescending(x => x.bikesSold).ToList();

            return View(homeDashboard);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}