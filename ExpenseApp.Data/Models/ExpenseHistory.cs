//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExpenseHistory
    {
        public int ID { get; set; }
        public int ExpenseId { get; set; }
        public int ExpenseStatusId { get; set; }
        public string RejectReason { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual Expense Expense { get; set; }
        public virtual ExpenseStatus ExpenseStatus { get; set; }
    }
}