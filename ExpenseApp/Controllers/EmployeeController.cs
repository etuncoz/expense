using System.Web.Mvc;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain.ViewModels;
using ExpenseApp.Engine.Handlers;
using System;
using ExpenseApp.Engine.Enum;

namespace ExpenseApp.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login", "Home");

            //Control for authorization
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Employee)
            {
                var viewModel = new UserViewModel
                {
                    ID = id
                };
                return View(viewModel);
            }

            return RedirectToAction("Login", "Home");
        }

        public ActionResult SaveExpense(int id, string userId)
        {
            // userId ya da id parametreleri yok kontrolü nasıl yapmalı?**

            //Control for authorization
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Employee 
                && Convert.ToInt32(Session["UserId"]) == Int32.Parse(userId))
            {
                //To avoid editing an expense that was sent for approval
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if ((currentStatusId == null && id == (int)StatusEnum.NotCreated)
                    || currentStatusId == (int)StatusEnum.Ongoing 
                    || currentStatusId == (int)StatusEnum.Rejected)
                {
                    var viewModel = new ExpenseViewModel
                    {
                        ID = id,
                        UserId = Int32.Parse(userId)
                    };
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Employee", new { id = Int32.Parse(userId) });
            }
            return RedirectToAction("Login", "Home");

        }
    }
}