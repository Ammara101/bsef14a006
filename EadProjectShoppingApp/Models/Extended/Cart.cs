using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models
{
    [MetadataType(typeof(CartMetaData))]

    public partial  class Cart
    {
        //[Key]
        //public int Id { get; set; }

        
        public int Count { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        
    }

    public class CartMetaData
    {
        public string CartId { get; set; }

        public int ProductId { get; set; }
        //    public int count;
        //    public int CartId { get; set; }
        //    public int ProductId { get; set; }
        //    public int UserId { get; set; }
        //    public DateTime AddedOn { get; set; }
        //    public DateTime UpdatedOn { get; set; }

        }
    }