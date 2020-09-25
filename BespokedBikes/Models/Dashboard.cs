using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BespokedBikes.Models
{
    public class Dashboard
    {
        [Key]
        public int id { get; set; }
        public List<CommissionReport> salesThisMonth { get; set; }
        public List<BikeSummary> hotBike { get; set; }
        public List<Discount> currentDiscounts { get; set; }
    }
}