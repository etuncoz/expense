using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ExpenseApp.EmailService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 
            {
                x.Service<JobScheduler>(s =>                               
                {
                    s.ConstructUsing(name => new JobScheduler());    
                    s.WhenStarted(js => js.Start());              
                    s.WhenStopped(js => js.Stop());              
                });
                x.RunAsLocalSystem();

                x.SetDescription("Manager Notification Service");
                x.SetDisplayName("Manager Email");
                x.SetServiceName("ManagerNotificationService");                       
            });                
        }
    }
}
