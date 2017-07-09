
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models
{
    [MetadataType(typeof(ProductMetaData))]
    public partial  class Product
    {
        //  public string ProductName { get; set; }

        //public string Category { get; set; }
        public string CategoryName { get; set; }

    }

    public class ProductMetaData
    {


        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(45, ErrorMessage = "The maximum length must be upto 45 characters only")]
        public string ProductName { get; set; }

        //[DisplayName("Category Name")]
        //[Required(ErrorMessage = "Category name is required")]
        //public string CategoryName { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Image")]
        public string ProductImage { get; set; }

        [DisplayName("Price")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Has to be decimal with two decimal points")]
        public decimal Price { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category name is required")]

        public string CategoryName { get; set; }

        // public string  Category { get; set; }



    }
}