using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
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
