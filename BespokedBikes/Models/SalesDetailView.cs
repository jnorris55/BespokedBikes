using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BespokedBikes.Models
{
    public class SalesDetailView
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        public Salesperson Salesperson { get; set; }
        public Customer Customer { get; set; }
        [DisplayName("Sale Date")]
        public System.DateTime Sales_Date { get; set; }
        [DisplayName("Sale Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Sale_Price {get; set;}
        [DisplayName("Commission")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Comission {get; set;}
    }
}