//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EadProjectShoppingApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderedProduct
    {
        public int Id { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> CustomerOrderId { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}
