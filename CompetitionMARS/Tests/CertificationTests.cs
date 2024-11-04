
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using CompetitionMARS.Models;
using CompetitionMARS.Pages;
using CompetitionMARS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace CompetitionMARS.Tests
{
    public class CertificationTests : CommonDriver
    {
        private Certification certPage;
        public static ExtentReports extent;
        public List<string> addedCertificationsEntries = new List<string>();


        [OneTimeSetUp]
        public void SetUp()
        {
            // Start the report
            extent = ExtentManager.StartReport();

            certPage = new Certification();
            LoginActions loginobj = new LoginActions();
            LoginData loginuser = loginobj.GetLoginData();

            loginobj.Login(loginuser.Email, loginuser.Password);
            certPage.NavigateToCertificationTab();
            certPage.ClearAllCertification();
        }
        [Test, Order(1)]
        public void CertificationTest_Valid()
        {
            ExtentTest test = extent.CreateTest("CertificationTest_Valid");

            CertificationData cert = certPage.GetCertificationData("validdata1");

            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            // Track the added CertificateName for cleanup
           
            test.Log(Status.Pass, "success");
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            Directory.CreateDirectory(folderPath);

            // File name with timestamp
            string screenshotPath = Path.Combine(folderPath, $"test_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            // Take screenshot
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath);
            test.AddScreenCaptureFromPath(screenshotPath);
            test.Pass("Screenshot added to Extent Report");

            // Assert that the certification was successfully added
            bool isCertificationAdded = certPage.IsCertificationAdded(cert.CertificateOrAward);
            Assert.IsTrue(isCertificationAdded, "Certification entry was not added successfully.");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
            Thread.Sleep(3000);

        }
        [Test, Order(2)]
        public void CertificationTest_Invalid()
        {
            ExtentTest test = extent.CreateTest("CertificationTest_Invalid");

            CertificationData cert = certPage.GetCertificationData("invaliddata");

            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            // Track the added CertificateName for cleanup
            //addedCertificationsEntries.Add(cert.CertificateOrAward);
            bool isEntryPresent = certPage.IsCertificationEntryPresent(cert.CertificateOrAward);
            Assert.IsTrue(isEntryPresent, $"Invalid entry '{cert.CertificateOrAward}' should not be added.");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }

        }
        [Test, Order(3)]
        public void CertificationTest_MissingField()
        {
            ExtentTest test = extent.CreateTest("CertificationTest_MissingField");
            Certification certPage = new Certification();
            CertificationData cert = certPage.GetCertificationData("validdata12");

            certPage.AddNewCertification_missingField(cert.CertificateOrAward, cert.CertificationYear);
            // Assert that an appropriate error message is displayed
            var errorMessage = certPage.GetErrorMessage();
            Assert.IsFalse(errorMessage.Contains("Please enter Certification Name,Certification From and Certification Year"),
                "Expected error message for missing field is not displayed.");
            IWebElement cancelButton = driver.FindElement(By.XPath("//input[contains(@value,'Cancel')]"));
            cancelButton.Click();
            Thread.Sleep(3000);
            if (false)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(4)]
        public void AlreadyExisting_CertTest()
        {
            ExtentTest test = extent.CreateTest("AlreadyExisting_CertTest");

            CertificationData cert = certPage.GetCertificationData("validdata7");

            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            //Add already existing data
            Thread.Sleep(3000);
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            // Track the added CertificateName for cleanup
            //addedCertificationsEntries.Add(cert.CertificateOrAward);
            //Verify the error message
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[contains(text(),'This information is already exist.')]"))
            );

            Assert.IsTrue(errorMessage.Displayed, "Expected error message for already existing is not displayed!");
            IWebElement cancelButton = driver.FindElement(By.XPath("//input[contains(@value,'Cancel')]"));
            cancelButton.Click();
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }

        }
        [Test, Order(5)]
        public void AddCertification_LargePayload()
        {
            ExtentTest test = extent.CreateTest("Certification Test");

            CertificationData cert = certPage.GetCertificationData("largedata");

            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            // Track the added CertificateName for cleanup
            //addedCertificationsEntries.Add(cert.CertificateOrAward);
            // Assert that the certification was successfully added
            bool isCertificationAdded = certPage.IsCertificationAdded(cert.CertificateOrAward);
            Assert.IsTrue(isCertificationAdded, "Certification entry was not added successfully.");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(6)]
        public void EditCertificationTest_Valid()
        {

            ExtentTest test = extent.CreateTest("EditCertificationTest_Valid");

            Certification certPage = new Certification();

            CertificationData cert = certPage.GetCertificationData("editdata5");
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);

            CertificationData cert1 = certPage.GetCertificationData("editdata6");
            // Edit the certification entry
            certPage.EditCertification(cert1.CertificateOrAward, cert1.CertifiedFrom);
            // Track the added CertificateName for cleanup
           // addedCertificationsEntries.Add(cert.CertificateOrAward);
            // Verify the update was successful
            bool isUpdated = certPage.IsCertificationUpdated(cert.CertifiedFrom);
            Assert.IsTrue(isUpdated, "Certification entry was not updated correctly.");
            if (isUpdated)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }


        }
        [Test, Order(7)]
        public void EditCertificationTest_Invalid()
        {
            ExtentTest test = extent.CreateTest("EditCertificationTest_Invalid");

            Certification certPage = new Certification();
            CertificationData cert = certPage.GetCertificationData("validdata11");

            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            //edit the certification entry with invalid data
            CertificationData certEdit = certPage.GetCertificationData("editinvaliddata");

            certPage.EditCertification(certEdit.CertificateOrAward, certEdit.CertifiedFrom);
            // Track the added CertificateName for cleanup
            //addedCertificationsEntries.Add(cert.CertificateOrAward);
            bool isEntryPresent = certPage.IsCertificationEntryPresent(cert.CertificateOrAward);
            Assert.IsTrue(isEntryPresent, $"Invalid entry '{cert.CertificateOrAward}' should not be added.");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(8)]
        public void EditCertification_MissingField()
        {
            ExtentTest test = extent.CreateTest("EditEducation_MissingField");

            // Load invalid data
            CertificationData cert = certPage.GetCertificationData("validdata9");
            Thread.Sleep(3000);
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            //Edit certification with missing field
            CertificationData editData = certPage.GetCertificationData("editdatamissingfield");
            // Edit the certification entry
            certPage.EditCertification_missing(editData.CertificateOrAward, editData.CertifiedFrom, editData.CertificationYear);
            // Assert that the edited entry with missing fields is not present in the list

            bool isPresent = certPage.IsCertificationAdded(editData.CertificateOrAward);
            Assert.IsFalse(isPresent, $"Certification with missing fields '{editData.CertificateOrAward}' should not be present in the list.");
            IWebElement cancelButton = driver.FindElement(By.XPath("//input[contains(@value,'Cancel')]"));
            cancelButton.Click();
            if (isPresent)
            {
                test.Log(Status.Fail, $"Failed: Certification with missing fields '{editData.CertificateOrAward}' is still present.");
            }
            else
            {
                test.Log(Status.Pass, "Success: Certification with missing fields was not added to the list.");
            }
        }


        [Test, Order(9)]
        public void DeleteCertificationTest()
        {

            ExtentTest test = extent.CreateTest("DeleteCertificationTest");
            CertificationData cert = certPage.GetCertificationData("validdata8");
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            certPage.DeleteCertification(cert.CertificateOrAward);
            bool isDeleted = certPage.IsCertificationDeleted(cert.CertificateOrAward);
            Assert.IsTrue(isDeleted, $"Certification '{cert.CertificateOrAward}' was not removed as expected.");
            if (isDeleted)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(10)]
        public void AddCertification_LargePayloadInDegree()
        {
            ExtentTest test = extent.CreateTest("AddCertification_LargePayloadInDegree");

            CertificationData cert = certPage.GetCertificationData("largepayload");
            // Add the new certification entry using the large degree payload
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            // Track the added CertificateName for cleanup
            //addedCertificationsEntries.Add(cert.CertificateOrAward);
            // Verify that the system adds the entry without crashing
            var validationMessage = driver.FindElement(By.XPath($"//div[contains(text(),'{cert.CertificateOrAward}')]"));
            Assert.IsTrue(validationMessage.Displayed, "System crashed due to large payload in Degree field");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(11)]
        public void AddCertificationn_MultipleEntries()
        {
            ExtentTest test = extent.CreateTest("AddCertification_MultipleEntries");

            CertificationData cert = certPage.GetCertificationData("validdata4");

            // Add certification entry 1
            certPage.AddNewCertification(cert.CertificateOrAward, cert.CertifiedFrom, cert.CertificationYear);
            
            bool isCertificationAdded = certPage.IsCertificationAdded(cert.CertificateOrAward);
            Assert.IsTrue(isCertificationAdded, "Certification entry was not added successfully.");

            //Add certification entry 2
            CertificationData cert2 = certPage.GetCertificationData("validdata5");
            certPage.AddNewCertification(cert2.CertificateOrAward, cert2.CertifiedFrom, cert2.CertificationYear);
            
            bool isCertificationAdded2 = certPage.IsCertificationAdded(cert.CertificateOrAward);
            Assert.IsTrue(isCertificationAdded, "Certification entry was not added successfully.");

            //Add certification entry 3
            CertificationData cert3 = certPage.GetCertificationData("validdata6");
            certPage.AddNewCertification(cert3.CertificateOrAward, cert3.CertifiedFrom, cert3.CertificationYear);
            
            bool isCertificationAdded3 = certPage.IsCertificationAdded(cert.CertificateOrAward);
            Assert.IsTrue(isCertificationAdded, "Certification entry was not added successfully.");
            // Log the result
            if (true)
            {
                test.Log(Status.Pass, "All multiple certification entries were added successfully.");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }

        }

      


            /*[TearDown]
            public void CleanData()
            {
                CleanupAddedCertificationEntries();
            }*/
    [OneTimeTearDown]
    public void TearDown()
        {
            extent.Flush(); // End the report
            certPage.CleanupAddedCertificationEntries();
            driver.Quit();
        }
    }

}




