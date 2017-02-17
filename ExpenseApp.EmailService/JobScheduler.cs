using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace ExpenseApp.EmailService
{
    public class JobScheduler
    {
        public void Start() 
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInHours(24)
                  .RepeatForever())
              .Build();

            sched.ScheduleJob(job, trigger);
        }
        public void Stop()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = schedFact.GetScheduler();
            sched.Shutdown();
        }

    }
}
