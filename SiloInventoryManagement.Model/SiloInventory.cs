//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiloInventoryManagement.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SiloInventory
    {
        public int SiloInventoryID { get; set; }
        public int RecordTypeID { get; set; }
        public int LocationID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public string EnteredBy { get; set; }
        public System.DateTime DateModified { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual RecordType RecordType { get; set; }
    }
}
