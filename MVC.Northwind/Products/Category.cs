using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Script.Serialization;

namespace RunningObjects.MVC.Northwind.Products
{
    public class Category : IValidatableObject
    {
        public Category()
        {
        }

        public Category
        (
            [Display(Name = "Name"), Required] string categoryName,
            [Display(Name = "Description"), Required] string description
        )
        {
            if (string.IsNullOrEmpty(categoryName))
                throw new ArgumentNullException("categoryName");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");

            CategoryName = categoryName;
            Description = description;
        }

        [Key, Display(Name = "#")]
        public int CategoryID { get; set; }

        [Text, Display(Name = "Name")]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        [ScriptIgnore]
        [Query(Select = "ProductID,ProductName,UnitPrice,Discontinued", OrderBy = "ProductName:Asc")]
        public virtual ICollection<Product> Products { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (validationContext.State() == EntityState.Deleted && Products.Count > 0)
                return new[] { new ValidationResult("There are products associated to this category") };
            return new[] { ValidationResult.Success };
        }
    }
}
