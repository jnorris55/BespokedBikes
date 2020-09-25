using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BespokedBikes.Models
{
    public class CommissionReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Bikes Sold")]
        public int bikesSold { get; set; }
        [DisplayName("Total Sales")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal totalSales { get; set; }
        [DisplayName("Total Commissions")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal totalComissions { get; set; }        
    }
}