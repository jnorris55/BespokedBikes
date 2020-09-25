using BespokedBikes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;


namespace BespokedBikes.Controllers
{
    public class AccessController : ApiController
    {

        private BikesDBEntities2 _db = new BikesDBEntities2();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET api/getSalespeople
        [HttpGet]
        public string getSalespeople(int id)
        {
            List<Salesperson> listSalesPeople = new List<Salesperson>();
            _db.Configuration.ProxyCreationEnabled = false;
            listSalesPeople = (from p in _db.Salespersons select p).ToList();
            
            var aSerializer = new XmlSerializer(typeof(List<Salesperson>));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            aSerializer.Serialize(sw, listSalesPeople);
            string apiResult = sw.GetStringBuilder().ToString();

            return apiResult;

        }

        // GET api/getCustomers
        public string getCustomers(int id)
        {
            List<Customer> listCustomers = new List<Customer>();
            _db.Configuration.ProxyCreationEnabled = false;
            listCustomers = (from p in _db.Customers select p).ToList();

            var aSerializer = new XmlSerializer(typeof(List<Customer>));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            aSerializer.Serialize(sw, listCustomers);
            string apiResult = sw.GetStringBuilder().ToString();
            
            return apiResult;
        }

        // GET api/getProducts
        public string getProducts(int id)
        {
            List<Product> listProducts = new List<Product>();
            _db.Configuration.ProxyCreationEnabled = false;
            listProducts = (from p in _db.Products select p).ToList();

            var aSerializer = new XmlSerializer(typeof(List<Product>));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            aSerializer.Serialize(sw, listProducts);
            string apiResult = sw.GetStringBuilder().ToString();

            return apiResult;
        }

        // GET api/getSales
        public string getSales(int id)
        {
            List<Sale> listSales = new List<Sale>();
            _db.Configuration.ProxyCreationEnabled = false;
            listSales = (from p in _db.Sales select p).ToList();

            var aSerializer = new XmlSerializer(typeof(List<Sale>));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            aSerializer.Serialize(sw, listSales);
            string apiResult = sw.GetStringBuilder().ToString();

            return apiResult;
        }

    }
}
