using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BueConsulting.Models;
using WebMatrix.WebData;
using BueConsulting.Filters;

namespace BueConsulting.Controllers
{

    public class ConsultantController : Controller
    {

        static BlueConsultingContext context = new BlueConsultingContext();

        public ConsultantController()
        {

        }

        // GET: /Consultant/
        [Authorize(Roles = "Consultant")]
        public ActionResult Index()
        {
            ViewBag.Message = "Consultant page.";

            return View();
        }

        /*
         * This method will return View with reports depending on the type of report
         * type: all reports, approved reports and pending reports
         */
        public ActionResult ViewReports(string name)
        {
            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);
            IEnumerable<Report> reports = null;

            if (name.Equals("All")) { reports = currentUser.Reports; }
            if (name.Equals("Approved")) { reports = currentUser.Reports.Where(x => x.AccountsApproval == "APPROVED"); }
            if (name.Equals("Pending")) { reports = currentUser.Reports.Where(x => x.AccountsApproval == "PENDING"); }

            return View(reports);
        }

        public ActionResult CreateReport()
        {
            return View();
        }

        /*
         * This method will return View with expenses model
         */
        public ActionResult ViewExpenses(int id)
        {
            var expenses = context.Reports.Find(id).Expenses;
            Session["CurrentReportID"] = id;
            return View(expenses);
        }

        /*
         * This method will create report
         */
        [HttpPost]
        public ActionResult CreateReport(FormCollection collection)
        {
            if (collection.Get("report name").Equals(""))
            {
                ViewBag.ErrorMessage = "Please enter a report name.";
                return View();
            }
            else
            {
                Session["ReportName"] = collection.Get("report name");
                Session["ExpensesList"] = new List<Expense>();
                Session["error"] = false;
                Session["listSize"] = 0;
                return RedirectToAction("AddExpenses");
            }
        }

        /*
         * This method will returns View call AddExpenses if their is no session called error
         */
        public ActionResult AddExpenses()
        {
            if ((bool)Session["error"])
            {
                ViewBag.ErrorMessage = "Can't add a report with no expenses.";
                return View();
            }
            else
            {
                return View();
            }
        }

        /*
         * This method will create the object of Expense and add it to the list
         */
        [HttpPost]
        public ActionResult AddExpenses(CreateExpense model)
        {
                Expense expense = new Expense() { Date = Convert.ToDateTime(model.Date),
                Description = model.Description,
                Location = model.Location,
                Amount = Math.Round(model.Amount, 2),
                Currency = model.Currency};
                expense.AmountAud = expense.convertCurrency(model.Currency, model.Amount);
                HttpPostedFileBase pdfFile = Request.Files["pdfUpload"];
                expense.Pdf = expense.convertFileToArray(pdfFile);
                var expenseList = (List<Expense>)Session["ExpensesList"];
                expenseList.Add(expense);
                Session["ExpensesList"] = expenseList;
                Session["listSize"] = expenseList.Count;
                return RedirectToAction("AddExpenses");
        }

        public ActionResult ViewReceipt(int id)
        {
            byte[] pdf = context.Expenses.Find(id).Pdf;
            return File(pdf, "application/pdf");
        }

        /*
         * This method will add report
         */
        [InitializeSimpleMembership]
        public ActionResult AddReport()
        {
            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);
            double totalAmount = 0;

            Report report = new Report();
            List<Expense> expenses = (List<Expense>)Session["ExpensesList"];
            if (expenses.Count == 0)
            {
                Session["error"] = true;
                return RedirectToAction("AddExpenses");
            }
            else
            {
                foreach (Expense exp in expenses)
                {
                    totalAmount += exp.AmountAud;
                }

                report.AccountsApproval = "PENDING";
                report.ConsultantId = currentUser.UserId;
                report.DepartmentName = currentUser.Department;
                report.TotalAmount = totalAmount;
                report.ReportName = Session["ReportName"].ToString();
                report.SupervisorApproval = "PENDING";
                report.Expenses = expenses;

                currentUser.Reports.Add(report);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        /*
         * method for testing
         */
        public ActionResult ViewReportsForTesting(string name, int userId)
        {
            UserProfile currentUser = context.UserProfiles.Find(userId);
            IEnumerable<Report> reports = null;

            if (name.Equals("All")) { reports = currentUser.Reports; }
            if (name.Equals("Approved")) { reports = currentUser.Reports.Where(x => x.AccountsApproval == "APPROVED"); }
            if (name.Equals("Pending")) { reports = currentUser.Reports.Where(x => x.AccountsApproval == "PENDING"); }

            return View(reports);
        }
    }
}
