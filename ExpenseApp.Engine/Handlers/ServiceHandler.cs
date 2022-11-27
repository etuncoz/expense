using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Response;
using System.Net.Mail;
using ExpenseApp.Engine.Enum;
using ExpenseApp.Data.Entities;

namespace ExpenseApp.Engine.Handlers
{
    public class ServiceHandler
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static BaseResponse CheckThenSendEmail(ExpenseDbContext entity,BaseResponse response)
        {

            var managers = (from u in entity.Users
                            where u.UserRoleId == (int)UserRoleEnum.Manager
                            select u).ToList().DefaultIfEmpty();
            if (managers == null)
            {
                response.IsSuccess = false;
                return response;
            }

            if (CheckIfManagerShouldBeEmailed(entity).IsNeeded)
            {
                foreach (var m in managers)
                    SendEmail(entity,m);
            }
            response.IsSuccess = true;
            return response;
        }
        public static void SendEmail(ExpenseDbContext entity, User user)
        {
            MailMessage mail = new MailMessage("noreply@veripark.com", user.Email);
            SmtpClient client = new SmtpClient();
            try
            {
                var config = entity.Configs.Where(c => c.ID == (int)ConfigEnum.PortConfig).First(); 
                String mailTemplate = "";
                client.Port = Int32.Parse(config.ConfigKey);
                client.Host = config.ConfigValue;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                mail.IsBodyHtml = true;
                mailTemplate = "Hello " + user.FullName + ",<br />" +
                    "There are expenses waiting for your approval. Please either approve or reject them as soon as possible.<br />" +
                    "Thank you";
                mail.Subject = "Expense Approval";
                mail.Body = mailTemplate;

                client.Send(mail);
            }
            catch (Exception ex)
            {
                log.Error("Sending Email Unsuccessful", ex);
            }
            finally
            {
                client.Dispose();
                mail.Dispose();
            }
            
        }
        public static ShouldCheckEmailResponse CheckIfManagerShouldBeEmailed(ExpenseDbContext entity)
        {
            ShouldCheckEmailResponse response = new ShouldCheckEmailResponse();

            var unapprovedExpenses = (from e in entity.GetUnApprovedExpenses
                                    select e).DefaultIfEmpty();
            if (unapprovedExpenses == null) 
            {
                response.IsNeeded = false;
                return response;
            }
            else
            {
                response.IsNeeded = true;
                return response;
            }
        }
    }
}
