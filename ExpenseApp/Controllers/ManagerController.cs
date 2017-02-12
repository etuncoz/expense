using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseApp.Engine.Domain.ViewModels;
using ExpenseApp.Engine.Handlers;

namespace ExpenseApp.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["UserRoleId"]) == 2)
                return View();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Approval(int id)
        {
            if (Convert.ToInt32(Session["UserRoleId"]) == 2)
            {
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if (currentStatusId == 2) //status = waiting for manager approval
                {
                    var viewModel = new ExpenseViewModel
                    {
                        ID = id
                    };
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Manager");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}