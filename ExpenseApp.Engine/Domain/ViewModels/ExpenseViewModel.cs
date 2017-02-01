using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpenseApp.Data.Models;

namespace ExpenseApp.Engine.Domain.ViewModels
{
    public class ExpenseViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }

        public int ID { get; set; }

        public decimal TotalAmount { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int LastExpenseActionId { get; set; }
    }
}