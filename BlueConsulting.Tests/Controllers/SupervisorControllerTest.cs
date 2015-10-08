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
    public class SupervisorControllerTest
    {
        [TestMethod]
        public void Supervisor_Initial()
        {

            SupervisorController controller = new SupervisorController();
            
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void Supervisor_ViewPendingReports()
        {
            SupervisorController controller = new SupervisorController();
            ViewResult result = controller.ViewReportsForTesting(19) as ViewResult;
       
            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

            if (reports != null)
                foreach (var report in reports)
                    Assert.AreEqual("PENDING", report.SupervisorApproval);
        }


        [TestMethod]
        public void ApproveReport()
        {
            SupervisorController controller = new SupervisorController();
            
            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.UpdateApprovalForTesting(46, "approve", 19) as ViewResult;
                BlueConsultingContext context = controller.Context();
                IEnumerable<Report> reports = context.Reports.Where(x => x.ReportId == 15);

                foreach (var report in reports)
                    Assert.AreEqual("APPROVED", report.SupervisorApproval);
                testTransaction.Dispose(); // rollback
            }
        }

        [TestMethod]
        public void RejectReport()
        {
            SupervisorController controller = new SupervisorController();

            using (TransactionScope testTransaction = new TransactionScope())
            {
                ViewResult result = controller.UpdateApprovalForTesting(46, "deny", 19) as ViewResult;
                BlueConsultingContext context = controller.Context();
                IEnumerable<Report> reports = context.Reports.Where(x => x.ReportId == 15);

                foreach (var report in reports)
                    Assert.AreEqual("REJECTED", report.SupervisorApproval);
                testTransaction.Dispose(); // rollback
            }
        }
        
        [TestMethod]
        public void AccountsRejected_ReturnsRejectedReports()
        {
            SupervisorController controller = new SupervisorController();
            ViewResult result = controller.AccountsRejectedForTesting(19) as ViewResult;

            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

            foreach (var report in reports)
                Assert.AreEqual("REJECTED", report.AccountsApproval);
        }
    }
}
