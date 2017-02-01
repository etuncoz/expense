using ExpenseApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApp.Engine.Domain

{
    public class ExpenseItemDto
    {
        public int ID { get; set; }
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime ExpenseItemDate { get; set; }

    }
}