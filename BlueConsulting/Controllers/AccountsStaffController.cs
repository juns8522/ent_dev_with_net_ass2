using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BueConsulting.Models;
using WebMatrix.WebData;
using BueConsulting.Filters;
using System.Data.Objects;
using System.Data.Common;

namespace BueConsulting.Controllers
{
    public class AccountsStaffController : Controller
    {
        static BlueConsultingContext context = new BlueConsultingContext();

        /* 
         * GET: /AccountsStaff/
         */
        [Authorize(Roles = "Accounts Staff")]
        public ActionResult Index()
        {
            CalculateExpenses();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "PENDING" && x.SupervisorApproval == "APPROVED");

            return View(reports);
        }

        /*
         * GET: /AccountsStaff/ViewExpenses/id
         */
        public ActionResult ViewExpenses(int id)
        {
            var expenses = context.Reports.Find(id).Expenses;
            return View(expenses);
        }

        /*
         *This method calculates expenses of all departments by calling DepExpenses method
         *and save the values into ViewBag.
         */
        public void CalculateExpenses()
        {
            double stateExpenses = DepExpenses("State Services");
            double logisticsExpenses = DepExpenses("Logistics Services");
            double higherEduExpenses = DepExpenses("Higher Education Services");
            double totalExpenses = stateExpenses + logisticsExpenses + higherEduExpenses;

            ViewBag.StateExpenses = stateExpenses;
            ViewBag.LogisticsExpenses = logisticsExpenses;
            ViewBag.HigherEduExpenses = higherEduExpenses;
            ViewBag.TotalExpenses = stateExpenses + logisticsExpenses + higherEduExpenses;
        }

        /*
         * This method is called by CalculateExpenses method.
         * This calculates total approved expenses for each department
         */ 
        public double DepExpenses(string department)
        {
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "APPROVED" && x.SupervisorApproval == "APPROVED" && x.DepartmentName == department);
            double totalExpenses = 0;

            foreach (Report report in reports)
            {
                totalExpenses += report.TotalAmount;
            }
            return totalExpenses;
        }

        /*
         * This method approve a specific report using reportId
         * return Index view with reports model
         */ 
        public ActionResult ApproveReport(int id)
        {
            context.Reports.Find(id).AccountsApproval = "APPROVED";
            context.SaveChanges();
            CalculateExpenses();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "PENDING" && x.SupervisorApproval == "APPROVED");

            return View("Index", reports);
        }

        /*
         * This method reject a specific report using reportId
         * return Index view with reports model
         */
        public ActionResult RejectReport(int id)
        {
            context.Reports.Find(id).AccountsApproval = "REJECTED";
            context.SaveChanges();
            CalculateExpenses();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "PENDING" && x.SupervisorApproval == "APPROVED");

            return View("Index", reports);
        }

        /*
         * GET: /AccountsStaff/ApprovedSupervisorDetail
         */
        public ActionResult ApprovedSupervisorDetail()
        {
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "APPROVED" && x.SupervisorApproval == "APPROVED");

            return View(reports);
        }

        /*
         * GET: /AccountsStaff/ViewReceipt/id
         */
        public ActionResult ViewReceipt(int id)
        {
            byte[] pdf = context.Expenses.Find(id).Pdf;
            return File(pdf, "application/pdf");
        }

        /*
         * returns BlueConsultingContext for testing
         */
        public BlueConsultingContext Context()
        {
            return context;
        }
    }
}
