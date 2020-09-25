using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BespokedBikes.Models
{
    public class ReportQuarter
    {
        [Key]
        public int id { get; set; }
        public List<string> quarterNames { get; set; }
        public List<CommissionReport> reportData { get; set; }
    }
}