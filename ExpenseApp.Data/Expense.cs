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
    
    public partial class Expense
    {
        public Expense()
        {
            UserId = 1;
            CreatedDate = DateTime.Now;;
            this.ExpenseHistories = new HashSet<ExpenseHistory>();
            this.ExpenseItems = new HashSet<ExpenseItem>();
        }
    
        public int ID { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastExpenseActionId { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<ExpenseHistory> ExpenseHistories { get; set; }
        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
    }
}