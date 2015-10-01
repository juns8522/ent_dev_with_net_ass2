using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;
using BueConsulting.Models;
using WebMatrix.WebData;
using BueConsulting.Filters;
using System.Configuration;

namespace BueConsulting.Controllers
{
    public class SupervisorController : Controller
    {
        static BlueConsultingContext context = new BlueConsultingContext();
        
        // GET: /Supervisor/
        [Authorize(Roles = "Supervisor")]
        public ActionResult Index()
        {
            ViewBag.Message = "Supervisor page";

            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);
            ViewBag.Department = currentUser.Department;

            // Select monthly budget remaining
            double total = context.Reports
                .Where(r => r.DepartmentName == currentUser.Department && r.SupervisorApproval.Equals("APPROVED") && !r.AccountsApproval.Equals("REJECTED"))
                .Select(r => r.TotalAmount)
                .ToList()
                .Sum();

            ViewBag.MonthlyExpenses = total;

            return View();
        }

        public ActionResult ViewReports()
        {
            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);
            IEnumerable<Report> reports = null;
            
            // Select monthly budget remaining
            double total = context.Reports
                .Where(r => r.DepartmentName == currentUser.Department && r.SupervisorApproval.Equals("APPROVED") && !r.AccountsApproval.Equals("REJECTED"))
                .Select(r => r.TotalAmount)
                .ToList()
                .Sum();

            ViewBag.MonthlyExpenses = total;

            reports = from r in context.Reports
                      where r.DepartmentName == currentUser.Department && r.SupervisorApproval == "PENDING"
                      select r;

            return View(reports);
        }

        public ActionResult ViewExpenses(int id)
        {
            var expenses = context.Reports.Find(id).Expenses;
            return View(expenses);
        }

        public ActionResult UpdateApproval(int id, String status)
        {
            var report = context.Reports.Find(id);
            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);

            switch (status)
            {
                case "approve":
                    report.SupervisorApproval = "APPROVED";
                    break;
                case "deny":
                    report.SupervisorApproval = "REJECTED";
                    break;
            }

            report.SupervisorId = currentUser.UserId;
            context.SaveChanges();

            return View();
        }

        public ActionResult AccountsRejected()
        {
            UserProfile currentUser = context.UserProfiles.Find(WebSecurity.CurrentUserId);
            IEnumerable<Report> reports = null;

            reports = from r in context.Reports
                      where r.DepartmentName == currentUser.Department && r.AccountsApproval == "REJECTED"
                      select r;

            return View(reports);
        }

        public ActionResult ViewReceipt(int id)
        {
            byte[] pdf = context.Expenses.Find(id).Pdf;
            return File(pdf, "application/pdf");
        }

        /*
         * Method for testing
         */
        public ActionResult IndexForTesting(int userId)
        {
            ViewBag.Message = "Supervisor page";

            UserProfile currentUser = context.UserProfiles.Find(userId);
            ViewBag.Department = currentUser.Department;

            // Select monthly budget remaining
            double total = context.Reports
                .Where(r => r.DepartmentName == currentUser.Department && r.SupervisorApproval.Equals("APPROVED") && !r.AccountsApproval.Equals("REJECTED"))
                .Select(r => r.TotalAmount)
                .ToList()
                .Sum();

            ViewBag.MonthlyExpenses = total;

            return View();
        }

        /*
         * Method for testing
         */
        public ActionResult ViewReportsForTesting(int userId)
        {
            UserProfile currentUser = context.UserProfiles.Find(userId);
            IEnumerable<Report> reports = null;

            // Select monthly budget remaining
            double total = context.Reports
                .Where(r => r.DepartmentName == currentUser.Department && r.SupervisorApproval.Equals("APPROVED") && !r.AccountsApproval.Equals("REJECTED"))
                .Select(r => r.TotalAmount)
                .ToList()
                .Sum();

            ViewBag.MonthlyExpenses = total;

            reports = from r in context.Reports
                      where r.DepartmentName == currentUser.Department && r.SupervisorApproval == "PENDING"
                      select r;

            return View(reports);
        }

        /*
         * Method for testing
         */
        public ActionResult UpdateApprovalForTesting(int reportId, String status, int userId)
        {
            var report = context.Reports.Find(reportId);
            UserProfile currentUser = context.UserProfiles.Find(userId);

            switch (status)
            {
                case "approve":
                    report.SupervisorApproval = "APPROVED";
                    break;
                case "deny":
                    report.SupervisorApproval = "REJECTED";
                    break;
            }

            report.SupervisorId = currentUser.UserId;
            context.SaveChanges();

            return View();
        }

        /*
         * Method for testing
         */
        public ActionResult AccountsRejectedForTesting(int userId)
        {
            UserProfile currentUser = context.UserProfiles.Find(userId);
            IEnumerable<Report> reports = null;

            reports = from r in context.Reports
                      where r.DepartmentName == currentUser.Department && r.AccountsApproval == "REJECTED"
                      select r;

            return View(reports);
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
