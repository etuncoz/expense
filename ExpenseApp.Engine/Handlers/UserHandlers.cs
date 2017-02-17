using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Request;
using System.Web.Mvc;
using ExpenseApp.Engine.Domain.Constants;

namespace ExpenseApp.Engine.Handlers
{
    public class UserHandlers
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LoginResponse Login(LoginRequest request)
        {
            ExpenseAppEntities entity = new ExpenseAppEntities();
            LoginResponse response = new LoginResponse();

            try
            {
                var user = (from u in entity.Users
                            where u.Email == request.Email && u.Password == request.Password
                            select u).SingleOrDefault();

                if (user != null)
                {
                    HttpContext.Current.Session[SessionConstants.UserId] = user.ID;
                    HttpContext.Current.Session[SessionConstants.UserEmail] = user.Email.ToString();
                    HttpContext.Current.Session[SessionConstants.UserName] = user.FullName.ToString();
                    HttpContext.Current.Session[SessionConstants.UserRoleId] = user.UserRoleId;

                    response.UserId = user.ID;
                    response.UserRoleId = user.UserRoleId;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Login Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        public static BaseResponse Logoff() 
        {
            ExpenseAppEntities entity = new ExpenseAppEntities();
            BaseResponse response = new BaseResponse();

            //Check if session exists
            if (HttpContext.Current.Session[SessionConstants.UserEmail] != null)
            {
                try 
	            {
                    //int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                    //entity.SaveChanges();
                    HttpContext.Current.Session.Abandon();
                    response.IsSuccess = true;
	            }
	            catch (Exception ex)
	            {
                    response.IsSuccess = false;
                    log.Error("Logoff Unsuccessful", ex);
	            }
                //finally
                //{
                //    entity.Dispose();
                //}
                return response;
            }
            response.IsSuccess = false;
            return response;
        }

        //public static UserResponse CheckIfUserIsActive()
        //{
        //    ExpenseAppEntities entity = new ExpenseAppEntities();
        //    UserResponse response = new UserResponse();
        //    try
        //    {
        //        if(HttpContext.Current.Session["UserId"]!=null)
        //        {
        //            response.IsActive = true;
        //        }
        //        response.IsActive = false;
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        entity.Dispose();
        //    }
        //    return response;
        //}
    }
}
