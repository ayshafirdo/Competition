using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  
    public class ExtentManager
    {
        public static ExtentReports extent;
        public static ExtentTest test;

       // [OneTimeSetUp]
        public void SetUp()
        {
            ExtentSparkReporter htmlReporter = new ExtentSparkReporter(@"Report.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }
     //   [OneTimeTearDown]
        public void TearDown()
        {
            extent.Flush();
        }
        public static ExtentReports StartReport()
        {
            var htmlReporter = new ExtentSparkReporter(@"Report.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            return extent;
        }

        public static void EndReport()
        {
            extent.Flush();
        }

        public static void CreateTest(string testName)
        {
            test = extent.CreateTest(testName);
        }

        public static void LogInfo(string message)
        {
            test.Info(message);
        }

        public static void LogPass(string message)
        {
            test.Pass(message);
        }

        public static void LogFail(string message)
        {
            test.Fail(message);
        }
    }


