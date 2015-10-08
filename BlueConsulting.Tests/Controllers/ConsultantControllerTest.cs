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
using Moq;
using System.Security.Principal;

namespace BueConsulting.Tests.Controllers
{
    [TestClass]
    public class ConsultantControllerTest
    {
        [TestMethod]
        public void Index_ReturnsController()
        {
            ConsultantController controller = new ConsultantController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(controller);
        }

        /*
         * This test calls ViewReports(string name, int userId) instead of ViewReports(string name)
         * to prevent HttpContext error
         */
        [TestMethod]
        public void ViewReports_ReturnsApprovedReports()
        {
            ConsultantController controller = new ConsultantController();
            ViewResult result = controller.ViewReportsForTesting("Approved", 1) as ViewResult;

            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

            if(reports != null)
                foreach (var report in reports)
                    Assert.AreEqual("APPROVED", report.AccountsApproval);
        }

        /*
         * This test calls ViewReports(string name, int userId) instead of ViewReports(string name)
         * to prevent HttpContext error
         */
        [TestMethod]
        public void ViewReports_ReturnsPenidingReports()
        {
            ConsultantController controller = new ConsultantController();
            ViewResult result = controller.ViewReportsForTesting("Pending", 1) as ViewResult;

            IEnumerable<BueConsulting.Models.Report> reports = (IEnumerable<BueConsulting.Models.Report>)result.Model;

            foreach (var report in reports)
            {
                Assert.AreEqual("PENDING", report.AccountsApproval);
            }
        }

        [TestMethod]
        public void CreateReport_ReturnsView()
        {
            ConsultantController controller = new ConsultantController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
