//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAS_WPF
{
    using System;
    using System.Collections.Generic;
    
    public partial class DrinkOrder
    {
        public System.Guid ID { get; set; }
        public System.Guid DrinkID { get; set; }
        public System.Guid OrderID { get; set; }
    
        public virtual Drink Drink { get; set; }
        public virtual Order Order { get; set; }
    }
}
