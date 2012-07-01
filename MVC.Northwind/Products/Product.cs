using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;

namespace RunningObjects.MVC.Northwind.Products
{
    [DisplayName("Product")]
    [Query(Select = "ProductID,ProductName,UnitPrice,Discontinued,Category", OrderBy = "ProductName:Asc", Paging = true, PageSize = 50)]
    public class Product
    {
        public Product()
        {
        }

        public Product
        (
            [Required, Display(Name = "Name")] string name,
            [Required, Display(Name = "Unit Price")] Decimal unitPrice,
            [Required, Display(Name = "Category")] Category category,
            bool discontinued
        )
        {
            ProductName = name;
            UnitPrice = unitPrice;
            CategoryID = category.CategoryID;
            Category = category;
            Discontinued = discontinued;
        }

        [Key, Display(Name = "#")]
        public int ProductID { get; set; }

        [Text, Display(Name = "Name")]
        public string ProductName { get; set; }

        public Decimal UnitPrice { get; set; }

        public bool Discontinued { get; set; }

        [ScaffoldColumn(false)]
        public int CategoryID { get; set; }

        [ScriptIgnore]
        [Display(Name = "Categoria")]
        public virtual Category Category { get; set; }

        public void Toggle()
        {
            Discontinued = !Discontinued;
        }

        public static List<Product> Show([Required] [Display(Name = "Please choose the Category")] Category category, [Display(Name = "Only Available Products")] bool onlyAvailableProducts)
        {
            var products = category.Products.AsQueryable();
            if (onlyAvailableProducts)
                products = products.Where(p => !p.Discontinued);
            return products.OrderBy(p => p.ProductName).ToList();
        }

        public override string ToString()
        {
            return ProductName;
        }
    }
}