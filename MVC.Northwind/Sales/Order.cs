using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Script.Serialization;

namespace RunningObjects.MVC.Northwind.Sales
{
    [Query(Select = "OrderID,Customer,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight", OrderBy = "OrderDate:Desc", Paging = true)]
    public class Order
    {
        [Key, Text]
        public int OrderID { get; set; }

        [ScaffoldColumn(false)]
        public string CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        [ScaffoldColumn(false)]
        public int? EmployeeID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? RequiredDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }

        [ScriptIgnore]
        [Query(Select = "Product,UnitPrice,Quantity,Discount")]
        public virtual ICollection<OrderDetail> Details { get; set; }

        public override string ToString()
        {
            return OrderID.ToString(CultureInfo.InvariantCulture);
        }
    }
}