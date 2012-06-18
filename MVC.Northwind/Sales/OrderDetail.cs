using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RunningObjects.MVC.Northwind.Products;

namespace RunningObjects.MVC.Northwind.Sales
{
    [DisplayName("Order Detail"), Query(Paging = true), ScaffoldTable(false)]
    public class OrderDetail
    {
        [Key, ScaffoldColumn(false)]
        public int OrderDetailID { get; set; }

        [ScaffoldColumn(false)]
        public int OrderID { get; set; }

        [ScaffoldColumn(false)]
        public int ProductID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public decimal UnitPrice { get; set; }
        public Int16 Quantity { get; set; }
        public Single Discount { get; set; }
    }
}
