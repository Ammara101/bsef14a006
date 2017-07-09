using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models
{
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }
    public class CategoryMetaData
    {

        [Display(Name ="Category ID")]
        public int CateegoryId { get; set; }
        [Display(Name = "Category Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category Name Required")]

        public string CategoryName { get; set; }
    }
}