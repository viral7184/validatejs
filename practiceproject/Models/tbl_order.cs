//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace practiceproject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_order
    {
        public int ID { get; set; }
        public Nullable<int> CUSTOMER_ID { get; set; }
        public string NAME { get; set; }
        public string PRICE { get; set; }
        public string QUANTITY { get; set; }
        public string AMOUNT { get; set; }
    
        public virtual tbl_customer tbl_customer { get; set; }
    }
}