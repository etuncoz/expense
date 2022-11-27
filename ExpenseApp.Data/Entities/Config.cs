using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public class Config
    {
        public int ID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
