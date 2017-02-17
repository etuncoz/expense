using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseApp.Engine.Domain.ViewModels;
using ExpenseApp.Engine.Handlers;
using ExpenseApp.Engine.Enum;

namespace ExpenseApp.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            //Control for authorization
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Manager)
                return View();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Approval(int id)
        {
            //Control for authorization
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Manager)
            {
                //To avoid reapprovement of an expense
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if (currentStatusId == (int)StatusEnum.WaitingForManagerApproval)
                {
                    var viewModel = new ExpenseViewModel
                    {
                        ID = id
                    };
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Manager");
            }
            return RedirectToAction("Login", "Home");
        }
    }
}