//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseApp.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExpenseStatu
    {
        public ExpenseStatu()
        {
            this.ExpenseHistories = new HashSet<ExpenseHistory>();
        }
    
        public int ID { get; set; }
        public string StatusName { get; set; }
    
        public virtual ICollection<ExpenseHistory> ExpenseHistories { get; set; }
    }
}
