using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ExpenseApp.Data;
using ExpenseApp.Engine.Handlers;
using ExpenseApp.Engine.Response;
using Quartz;
using Quartz.Impl;
using log4net;

namespace ExpenseApp.WindowsService
{
    public class EmailJob : IJob
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();
            try
            {
                ServiceHandler.CheckThenSendEmail(entity,response);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                entity.Dispose();
            }
        }
    }
}
