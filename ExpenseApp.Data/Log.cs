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
    
    public partial class Log
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public System.DateTime LogDate { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
