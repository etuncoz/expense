using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ExpenseApp.WindowsService
{
    //protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public partial class Service1 : ServiceBase
    {
        JobScheduler scheduler;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            scheduler = new JobScheduler();
            scheduler.Start();
        }
        protected override void OnStop()
        {
            if(scheduler!=null)
                scheduler.Stop();
        }
    }
}
