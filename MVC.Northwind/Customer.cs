using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Script.Serialization;
using RunningObjects.MVC.Northwind.Sales;

namespace RunningObjects.MVC.Northwind
{
    [Query(OrderBy = "CompanyName:Asc", Paging = true, PageSize = 50)]
    public class Customer : IValidatableObject
    {
        [Key]
        public string CustomerID { get; set; }

        [Text, Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        [ScriptIgnore]
        [Query(Select = "OrderID,OrderDate,RequiredDate,ShippedDate", OrderBy = "OrderDate:Desc")]
        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return CompanyName;
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if(validationContext.State() == EntityState.Deleted && Orders.Count > 0)
                return new[] { new ValidationResult("There are orders associated to this customer.") };
            return new[] { ValidationResult.Success };
        }
    }
}