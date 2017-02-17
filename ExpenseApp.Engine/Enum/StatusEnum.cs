using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Engine.Enum
{
        public enum StatusEnum
        {
            NotCreated = 0,
            Ongoing = 1,
            WaitingForManagerApproval = 2,
            WaitingForAccountantApproval = 3,
            Completed = 4,
            Rejected = 5
        }
}
