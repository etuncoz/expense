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

namespace ExpenseApp.Engine.Handlers
{
	public class UserHandlers
	{
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
					HttpContext.Current.Session["UserId"] = user.ID;
				    HttpContext.Current.Session["UserEmail"] = user.Email;
				    HttpContext.Current.Session["UserName"] = user.FullName;
					HttpContext.Current.Session["UserRoleId"] = user.UserRoleId;

					user.IsActive = true;
					entity.SaveChanges();

					switch (user.UserRoleId)
					{
						case 1: response.RedirectionUrl = "/employee/index/" + user.ID;
							break;
						case 2: response.RedirectionUrl = "/manager/index";
							break;
						case 3: response.RedirectionUrl = "/accountant/index";
							break;
					}
				}
				else
				{
					response.RedirectionUrl = "/home/login";
				}

				response.IsSuccess = true;
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				throw ex;
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

			if (HttpContext.Current.Session["UserName"] != null)
			{
				try 
				{
					int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

					var user = (from u in entity.Users
								where u.ID == userId
								select u).Single();
					user.IsActive = false;
					entity.SaveChanges();
					HttpContext.Current.Session.Abandon();
					response.IsSuccess = true;
				}
				catch (Exception)
				{
					response.IsSuccess = false;
					throw;
				}
				finally
				{
					entity.Dispose();
				}
				return response;
			}
			response.IsSuccess = false;
			return response;
		}

		public static UserResponse CheckIfUserIsActive()
		{
			ExpenseAppEntities entity = new ExpenseAppEntities();
			UserResponse response = new UserResponse();
			try
			{
				if(HttpContext.Current.Session["UserId"]!=null)
				{
					response.IsActive = true;
				}
				response.IsActive = false;
				response.IsSuccess = true;
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				throw ex;
			}
			finally
			{
				entity.Dispose();
			}
			return response;
		}
	}
}
