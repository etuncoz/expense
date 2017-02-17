using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Engine.Request
{
    public class ExpenseApprovalRequest
    {
        public int ExpenseId { get; set; }
        public bool IsApproved { get; set; } 
        public string RejectReason { get; set; }
    }
}
