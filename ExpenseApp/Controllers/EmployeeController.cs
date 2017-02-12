using System.Web.Mvc;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain.ViewModels;
using ExpenseApp.Engine.Handlers;
using System;

namespace ExpenseApp.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            if (Convert.ToInt32(Session["UserRoleId"]) == 1)
            {
                var viewModel = new UserViewModel
                {
                    ID = id
                };
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SaveExpense(int id,string userId)
        {
                

            if (Convert.ToInt32(Session["UserRoleId"]) == 1 && Convert.ToInt32(Session["UserId"])== Int32.Parse(userId))
            {
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if ((currentStatusId == null && id == 0) || currentStatusId == 1)
                {
                    var viewModel = new ExpenseViewModel
                    {
                        ID = id,
                        UserId = Int32.Parse(userId)
                    };
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Employee");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}