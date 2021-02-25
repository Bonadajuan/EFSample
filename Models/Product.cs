using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFSample.Models 
{ 
    [Table("Product", Schema = "SalesLT")] 
    public partial class Product 
    { 
        [Key] 
        public int ? ProductID { get; set; }        
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public decimal ? Weight { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime ? SellEndDate { get; set; }
        public DateTime ? DiscontinuedDate { get; set; }
        
        public override string ToString() 
        { 
            return Name;
        }
    }
}