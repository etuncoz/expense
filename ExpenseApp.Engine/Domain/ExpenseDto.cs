using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data.Models;

namespace ExpenseApp.Engine.Domain
{
    public class ExpenseDto
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
