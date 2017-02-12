using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseApp.Engine.Domain.ViewModels;
using ExpenseApp.Engine.Handlers;

namespace ExpenseApp.Controllers
{
    public class AccountantController : Controller
    {
        // GET: Accountant
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["UserRoleId"]) == 3)
                return View();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Payment(int id)
        {
            if (Convert.ToInt32(Session["UserRoleId"]) == 3)
            {
                int? currentStatusId = ExpenseActionHandlers.GetCurrentExpenseStatus(id).LastExpenseActionId;

                if (currentStatusId == 3) // status = waiting for accountant approval
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