using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BueConsulting;
using BueConsulting.Models;
using BueConsulting.Controllers;
using System.Transactions;

namespace BueConsulting.Tests.Controllers
{
    [TestClass]
    public class AccountsStaffControllerTest
    {
        [TestMethod]
        public void Index_ReturnsController()
        {
            AccountsStaffController controller = new AccountsStaffController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void Index_ReturnsModel_AccountsApprovalIsPendingSupervisorApprovalIsApproved()
        {
            AccountsStaffController controller = new AccountsStaffController();
            ViewResult result = controller.Index() as ViewResult;

            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;
            foreach (var report in reports)
            {
                Assert.AreEqual("PENDING", report.AccountsApproval);
                Assert.AreEqual("APPROVED", report.SupervisorApproval);
            }
        }

        [TestMethod]
        public void DepExpenses_StateServices()
        {
            AccountsStaffController controller = new AccountsStaffController();

            BlueConsultingContext context = controller.Context();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "APPROVED" && x.SupervisorApproval == "APPROVED" && x.DepartmentName == "State Services");

            double totalExpenses = 0;
            foreach (var report in reports)
                totalExpenses += report.TotalAmount;

            Assert.AreEqual(totalExpenses, controller.DepExpenses("State Services"));
        }

        [TestMethod]
        public void DepExpenses_LogisticsServices()
        {
            AccountsStaffController controller = new AccountsStaffController();

            BlueConsultingContext context = controller.Context();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "APPROVED" && x.SupervisorApproval == "APPROVED" && x.DepartmentName == "Logistics Services");

            double totalExpenses = 0;
            foreach (var report in reports)
                totalExpenses += report.TotalAmount;

            Assert.AreEqual(totalExpenses, controller.DepExpenses("Logistics Services"));
        }

        [TestMethod]
        public void DepExpenses_HigherEducationServices()
        {
            AccountsStaffController controller = new AccountsStaffController();

            BlueConsultingContext context = controller.Context();
            IEnumerable<Report> reports = context.Reports.Where(x => x.AccountsApproval == "APPROVED" && x.SupervisorApproval == "APPROVED" && x.DepartmentName == "Higher Education Services");

            double totalExpenses = 0;
            foreach (var report in reports)
                totalExpenses += report.TotalAmount;

            Assert.AreEqual(totalExpenses, controller.DepExpenses("Higher Education Services"));
        }

        [TestMethod]
        public void ApproveReport()
        {
            AccountsStaffController controller = new AccountsStaffController();
            IEnumerable<BueConsulting.Models.Report> reports;

            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.ApproveReport(46) as ViewResult;
                reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

                testTransaction.Dispose(); // rollback
            }

            reports = from rep in reports where rep.ReportId == 1 select rep;

            foreach (var report in reports)
                Assert.AreEqual("APPROVED", report.AccountsApproval);
        }

        [TestMethod]
        public void ApproveReport_ReturnsModel_AccountsApprovalIsPendingSupervisorApprovalIsApproved()
        {
            AccountsStaffController controller = new AccountsStaffController();
            IEnumerable<BueConsulting.Models.Report> reports;

            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.ApproveReport(46) as ViewResult;
                reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

                testTransaction.Dispose(); // rollback
            }

            foreach (var report in reports)
            {
                Assert.AreEqual("PENDING", report.AccountsApproval);
                Assert.AreEqual("APPROVED", report.SupervisorApproval);
            }
        }

        [TestMethod]
        public void RejectReport()
        {
            AccountsStaffController controller = new AccountsStaffController();
            IEnumerable<BueConsulting.Models.Report> reports;

            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.RejectReport(46) as ViewResult;
                reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

                testTransaction.Dispose(); // rollback
            }
            reports = from rep in reports where rep.ReportId == 1 select rep;

            foreach (var report in reports)
                Assert.AreEqual("REJECTED", report.AccountsApproval);
        }

        [TestMethod]
        public void RejectReport_ReturnsModel_AccountsApprovalIsPendingSupervisorApprovalIsApproved()
        {
            AccountsStaffController controller = new AccountsStaffController();
            IEnumerable<BueConsulting.Models.Report> reports;

            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.RejectReport(46) as ViewResult;
                reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

                testTransaction.Dispose(); // rollback
            }

            foreach (var report in reports)
            {
                Assert.AreEqual("PENDING", report.AccountsApproval);
                Assert.AreEqual("APPROVED", report.SupervisorApproval);
            }
        }

        [TestMethod]
        public void ApprovedSupervisorDetail()
        {
            AccountsStaffController controller = new AccountsStaffController();
            ViewResult result = controller.ApprovedSupervisorDetail() as ViewResult;

            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;
            foreach (var report in reports)
            {
                Assert.AreEqual("APPROVED", report.SupervisorApproval);
            }
        }
    }
}
