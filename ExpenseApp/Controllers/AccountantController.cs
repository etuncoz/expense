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
    public class AccountantController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Accountant)
                return View();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Payment(int id)
        {
            //Control for authorization
            if (Convert.ToInt32(Session["UserRoleId"]) == (int)UserRoleEnum.Accountant)
            {
                //To avoid repaying an expense
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if (currentStatusId == (int)StatusEnum.WaitingForAccountantApproval) 
                {
                    var viewModel = new ExpenseViewModel
                    {
                        ID = id
                    };
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Accountant");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}