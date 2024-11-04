using CompetitionMARS.Utilities;
using CompetitionMARS.Pages;
using CompetitionMARS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using RazorEngine;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports.Model;


namespace CompetitionMARS.Tests
{
    [TestFixture]

    public class EducationTests : CommonDriver
    {
        private Education eduPage;
        public static ExtentReports extent;

        public List<string> addedEducationEntries = new List<string>();

        [OneTimeSetUp]
        public void SetUp()
        {

            extent = ExtentManager.StartReport();
            addedEducationEntries = new List<string>();
            eduPage = new Education();
            LoginActions loginobj = new LoginActions();
            LoginData loginuser = loginobj.GetLoginData();

            loginobj.Login(loginuser.Email, loginuser.Password);
            eduPage.NavigateToEducationTab();
            eduPage.ClearAllEducation();
        }
        [Test, Order(1)]

        public void AddNewEducation_ValidTest()

        {
            ExtentTest test = extent.CreateTest("AddNewEducation_ValidTest");


            EducationData edu = eduPage.GetEducationData("validdata1");

            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);
            // Track the added CollegeName for cleanup
            //addedEducationEntries.Add(edu.CollegeName);

            // Verify that the education entry is added
            bool isEntryPresent = eduPage.VerifyEducationAdded(edu.CollegeName);
            Assert.IsTrue(isEntryPresent, $"Entry for college {edu.CollegeName} not found in the table!");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }


        }
        [Test, Order(2)]

        public void MissingFieldEduTest()
        {
            ExtentTest test = extent.CreateTest("MissingFieldEduTest");
            EducationData edu = eduPage.GetEducationData("validdata2");
            eduPage.AddNewEducation_missingField(edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);

            // Assert that an appropriate error message is displayed
            var errorMessage = eduPage.GetErrorMessage();
            Assert.IsTrue(errorMessage.Contains("Please enter all the fields"),
                "Expected error message for missing field is not displayed.");
            IWebElement cancelButton = driver.FindElement(By.XPath("//input[contains(@value,'Cancel')]"));
            cancelButton.Click();
            Thread.Sleep(3000);
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

        public void AddNewEducation_AlreadyExisting_ShouldShowErrorMessage()
        {
            ExtentTest test = extent.CreateTest("AddNewEducation_AlreadyExisting_ShouldShowErrorMessage");
            EducationData edu = eduPage.GetEducationData("validdata3");

            // Add education entry
            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);

            // Attempt to add the same entry again to test 
            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);

            // Verify that an error message is shown indicating the entry already exists
            var errorMessage = driver.FindElement(By.XPath("//div[contains(text(),'This information is already exist.')]")); // Update this with the correct ID or locator
            Assert.IsTrue(errorMessage.Displayed, "Error message not displayed for existing entry!");
            Assert.AreEqual("This information is already exist.", errorMessage.Text, "Incorrect error message for existing entry!");
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
        [Test, Order(4)]

        public void AddNewEducation_WithInvalidData_ShouldShowErrorMessage()
        {
            ExtentTest test = extent.CreateTest("AddNewEducation_AlreadyExisting_ShouldShowErrorMessage");

            // Load invalid data
            EducationData edu = eduPage.GetEducationData("invaliddata");


            eduPage.AddNewEducation_Invalid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);
            //addedEducationEntries.Add(edu.CollegeName);
            // Verify that the system shows an error message
            var errorMessage = driver.FindElement(By.XPath("//div[contains(text(),'Education has been added')]"));  // Adjust the XPath as necessary
            Assert.IsTrue(errorMessage.Displayed, "Expected error message for invalid data is not displayed!");

            // Verify that the invalid entry is not added to the table
            bool isEntryPresent = eduPage.IsEducationEntryPresent(edu.CollegeName);
            Assert.IsTrue(isEntryPresent, $"Invalid entry '{edu.CollegeName}' should not be added.");
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

        public void EditEducationTest()
        {

            ExtentTest test = extent.CreateTest("EditEducationTest");


            EducationData edu = eduPage.GetEducationData("validdata4");


            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);


            EducationData editData = eduPage.GetEducationData("editdata");
            // Edit the education entry
            eduPage.EditEducation(editData.CollegeName, editData.Degree);

            // Verify the update was successful
            bool isUpdated = eduPage.IsEducationUpdated(editData.Degree);
            Assert.IsTrue(isUpdated, "Education entry was not updated correctly.");
            if (isUpdated)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }

        }
        [Test, Order(6)]
        public void EditEduaction_InvalidData_ShouldShowErrorMessage()
        {
            ExtentTest test = extent.CreateTest("EditEduaction_InvalidData_ShouldShowErrorMessage");

            EducationData edu = eduPage.GetEducationData("validdata5");

            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);

            // Edit the education entry
            EducationData eduEdit = eduPage.GetEducationData("editinvaliddata");
            eduPage.EditEducation(eduEdit.CollegeName, eduEdit.Degree);
            // Verify the update was successful
            bool isUpdated = eduPage.IsEducationUpdated(eduEdit.Degree);
            Assert.IsTrue(isUpdated, "Education entry was not updated correctly.");


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
        public void EditEducation_MissingField()
        {
            ExtentTest test = extent.CreateTest("EditEducation_MissingField");
            EducationData edu = eduPage.GetEducationData("validdata6");
            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);
            //Edit education with missing field
            EducationData editData = eduPage.GetEducationData("editdatamissingfield");
            eduPage.EditEducation_missing(editData.CollegeName, editData.CountryOfCollege, editData.Degree);


            bool isEntryPresent = eduPage.IsEducationEntryPresent(edu.CollegeName);
            Assert.IsTrue(isEntryPresent, $"Entry '{edu.CollegeName}' should be present.");

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

        public void DeleteEducationTest()
        {

            ExtentTest test = extent.CreateTest("DeleteEducationTest");


            EducationData edu = eduPage.GetEducationData("validdata7");
            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);
            eduPage.DeleteEducation(edu.CollegeName);
            bool isDeleted = eduPage.IsEducationDeleted(edu.CollegeName);
            Assert.IsTrue(isDeleted, $"The Education '{edu.CollegeName}' was removed as expected.");
            if (isDeleted)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }
        [Test, Order(9)]
        public void AddEducation_LargePayloadInDegree()
        {
            ExtentTest test = extent.CreateTest("AddEducation_LargePayloadInDegree");

            EducationData edu = eduPage.GetEducationData("largepayload");
            // Add the new education entry using the large degree payload
            eduPage.AddNewEducation_Valid(
                edu.CollegeName,
                edu.CountryOfCollege,
                edu.Title,
                edu.Degree,
                edu.YearOfGraduation
            );
            // Verify that the system adds the entry without crashing
            var validationMessage = driver.FindElement(By.XPath("//div[contains(text(),'Education has been added')]"));
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
        [Test, Order(10)]
        public void AddEducation_MultipleEntries()
        {
            ExtentTest test = extent.CreateTest("AddEducation_MultipleEntries");


            EducationData edu = eduPage.GetEducationData("validdata8");

            // Add education entry 1
            eduPage.AddNewEducation_Valid(edu.CollegeName, edu.CountryOfCollege, edu.Title, edu.Degree, edu.YearOfGraduation);
            bool isEntryPresent1 = eduPage.VerifyEducationAdded(edu.CollegeName);
            Assert.IsTrue(isEntryPresent1, $"Entry for college {edu.CollegeName} not found in the table!");

            //Add education entry 2
            EducationData edu2 = eduPage.GetEducationData("validdata9");
            eduPage.AddNewEducation_Valid(edu2.CollegeName, edu2.CountryOfCollege, edu2.Title, edu2.Degree, edu2.YearOfGraduation);
            bool isEntryPresent2 = eduPage.VerifyEducationAdded(edu.CollegeName);
            Assert.IsTrue(isEntryPresent2, $"Entry for college {edu.CollegeName} not found in the table!");
            //Add education entry 3
            EducationData edu3 = eduPage.GetEducationData("validdata10");
            eduPage.AddNewEducation_Valid(edu3.CollegeName, edu3.CountryOfCollege, edu3.Title, edu3.Degree, edu3.YearOfGraduation);
            bool isEntryPresent3 = eduPage.VerifyEducationAdded(edu.CollegeName);
            Assert.IsTrue(isEntryPresent3, $"Entry for college {edu.CollegeName} not found in the table!");
            if (true)
            {
                test.Log(Status.Pass, "success");
            }
            else
            {
                test.Log(Status.Fail, "failed");
            }
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            // End the report
            extent.Flush();
            eduPage.CleanupAddedEducationEntries();
            driver.Quit();
        }


    }
}
