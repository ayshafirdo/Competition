using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using CompetitionMARS.Utilities;
namespace CompetitionMARS.Tests
{
   /* public class BaseTest : CommonDriver
    {

        protected ExtentReports extent;
        protected ExtentTest test;*/

      /*  [OneTimeSetUp]
        public void SetupReport()
        {
            var htmlReporter = new ExtentSparkReporter("extentReport.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:5000/Home");

            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [OneTimeTearDown]
        public void FlushReport()
        {
            extent.Flush();
        }
    }*/
}
