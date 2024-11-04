using CompetitionMARS.Models;
using CompetitionMARS.Utilities;
using Microsoft.CodeAnalysis;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RazorEngine.Compilation.ImpromptuInterface.Optimization;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompetitionMARS.Pages
{
    public class Education : CommonDriver
    {
        WaitUtils utils = new WaitUtils();
        private WebDriverWait wait;
        public List<string> addedEducationEntries = new List<string>(); // To track added entries



        private IWebElement EducationTab => driver.FindElement(By.XPath("//a[contains(text(),'Education') and @class='item']"));
        private IWebElement AddNewButton => driver.FindElement(By.XPath("//thead/tr[1]/th[6]/div[1]"));
        private IWebElement CollegeNameInput => driver.FindElement(By.XPath("//input[contains(@placeholder,'College/University Name')]"));
        private IWebElement CountryDropdown => driver.FindElement(By.XPath("//select[@class='ui dropdown' and @name='country']"));
        private IWebElement CountryDropdownEdit => driver.FindElement(By.XPath("//tbody/tr[1]/td[1]/div[1]/div[2]/select[1]"));
        private IWebElement TitleDropdown => driver.FindElement(By.XPath("//select[@class='ui dropdown' and @name='title']"));
        private IWebElement DegreeInput => driver.FindElement(By.XPath("//input[contains(@placeholder,'Degree')]"));
        private IWebElement YearOfGraduationDropdown => driver.FindElement(By.XPath("//select[@class='ui dropdown' and @name='yearOfGraduation']"));
        private IWebElement AddNewEducation => driver.FindElement(By.XPath("//input[contains(@value, 'Add')]"));
        private By eduElementTemp(string educationName) => By.XPath($"//td[text()='{educationName}']");
        private IWebElement EduElement(string educationName) => utils.WaitToBeVisible(eduElementTemp(educationName), 10);
        private IWebElement CancelButton => driver.FindElement(By.XPath("//input[contains(@value, 'Cancel')]"));
        private IWebElement EditButton(string collegeName) => driver.FindElement(By.XPath($"//tr[td[2][contains(text(), '{collegeName}')]]/td[6]/span[1]/i[1]"));
        private IWebElement UpdateButton => driver.FindElement(By.XPath("//input[contains(@value,'Update')]"));
        private By degreeElementTemp(string degreeName) => By.XPath($"//td[text()='{degreeName}']");
        private IWebElement DegreeElement(string degreeName) => utils.WaitToBeVisible(degreeElementTemp(degreeName), 10);
        private By entryElementTemp(string collegeName) => By.XPath($"//tr/td[contains(text(), '{collegeName}')]");
        private IWebElement EntryElement(string collegeName) => utils.WaitToBeVisible(entryElementTemp(collegeName), 10);
        private IWebElement TableElement => driver.FindElement(By.XPath("//body/div[@id='account-profile-section']/div[1]/section[2]/div[1]/div[1]/div[1]/div[3]/form[1]/div[4]/div[1]/div[2]"));
        //private IWebElement TableElement(string collegeName) => utils.WaitToBeVisible(tableElementTemp(collegeName), 10);
        private IWebElement DeleteButton(String collegeName) => driver.FindElement(By.XPath($"//tr[td[2][contains(text(), '{collegeName}')]]/td[6]/span[2]/i[1]"));
        private IWebElement ErrorElement => driver.FindElement(By.XPath("//div[contains(text(),'Please enter all the fields')]"));
        private IWebElement EduEntry => driver.FindElement(By.XPath("//th[contains(text(),'University')]/ancestor::table//tbody/tr/td[2]"));

        // Constructor to initialize WebDriver and WebDriverWait
        public Education()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public EducationData GetEducationData(String id)
        {
            // Path to your JSON file
            string jsonFilePath = @"Education.json";

            List<EducationData> educationDataList = JsonReader.ReadJsonFile<List<EducationData>>(jsonFilePath);
            EducationData edu = educationDataList.FirstOrDefault(e => e.id == id);
            return edu;
        }


        public void NavigateToEducationTab()
        {
            try
            {

                var educationTab = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a.item[data-tab='third']")));

                // Check if the tab is already active
                bool isTabActive = educationTab.GetAttribute("class").Contains("active");

                // Click on the tab only if it's not active
                if (!isTabActive)
                {

                    wait.Until(ExpectedConditions.ElementToBeClickable(educationTab)).Click();
                }
            }
            catch (WebDriverTimeoutException)
            {

                throw new Exception("The Education tab was not found or not clickable within the expected time.");
            }
        }
        public void AddNewEducation_Valid(string CollegeName, string CountryOfCollege, string Title, string Degree, int YearOfGraduation)
        {


            NavigateToEducationTab();

            AddNewButton.Click();

            CollegeNameInput.Click();
            CollegeNameInput.SendKeys(CollegeName);

            CountryDropdown.Click();
            SelectElement selectCountry = new SelectElement(CountryDropdown);
            selectCountry.SelectByText(CountryOfCollege);

            TitleDropdown.Click();
            SelectElement selectTitle = new SelectElement(TitleDropdown);
            selectTitle.SelectByText(Title);

            DegreeInput.Click();
            DegreeInput.SendKeys(Degree);

            YearOfGraduationDropdown.Click();
            SelectElement selectYear = new SelectElement(YearOfGraduationDropdown);
            selectYear.SelectByValue(YearOfGraduation.ToString());

            AddNewEducation.Click();

        }
        public void AddNewEducation_Invalid(string CollegeName, string CountryOfCollege, string Title, string Degree, int YearOfGraduation)
        {

            NavigateToEducationTab();

            AddNewButton.Click();

            CollegeNameInput.Click();
            CollegeNameInput.SendKeys(CollegeName);

            CountryDropdown.Click();
            SelectElement selectCountry = new SelectElement(CountryDropdown);
            selectCountry.SelectByText(CountryOfCollege);

            TitleDropdown.Click();
            SelectElement selectTitle = new SelectElement(TitleDropdown);
            selectTitle.SelectByText(Title);

            DegreeInput.Click();
            DegreeInput.SendKeys(Degree);

            YearOfGraduationDropdown.Click();
            SelectElement selectYear = new SelectElement(YearOfGraduationDropdown);
            selectYear.SelectByValue(YearOfGraduation.ToString());

            AddNewEducation.Click();
            addedEducationEntries.Add(CollegeName);
        }
        public bool IsEducationEntryPresent(string collegeName)
        {
            try
            {

                var entry = EntryElement(collegeName);
                return entry.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool VerifyEducationAdded(string collegeName)
        {


            //Verify that the education entry is present in the table

            var table = TableElement;
            bool isEntryPresent = true;

            // Find all rows in the table
            var rows = table.FindElements(By.TagName("tr"));

            // Loop through the rows and check if any row contains the college name
            foreach (var row in rows)
            {
                if (row.Text.Contains(collegeName))
                {
                    isEntryPresent = true;
                    break;
                }
            }
            return isEntryPresent;


        }
        public void AddNewEducation_missingField(string CountryOfCollege, string Title, string Degree, int YearOfGraduation)
        {

            AddNewButton.Click();
            CountryDropdown.Click();
            SelectElement selectCountry = new SelectElement(CountryDropdown);
            selectCountry.SelectByText(CountryOfCollege);

            TitleDropdown.Click();
            SelectElement selectTitle = new SelectElement(TitleDropdown);
            selectTitle.SelectByText(Title);

            DegreeInput.Click();
            DegreeInput.SendKeys(Degree);

            YearOfGraduationDropdown.Click();
            SelectElement selectYear = new SelectElement(YearOfGraduationDropdown);
            selectYear.SelectByValue(YearOfGraduation.ToString());

            AddNewEducation.Click();

        }
        public string GetErrorMessage()
        {
            try
            {
                // Locate the error message element 

                var errorElement = ErrorElement;
                return errorElement.Text; // Return the error message text
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }

            public void EditEducation_missing(string CollegeName, string CountryOfCollege, string Degree)
            {
                EditButton(CollegeName).Click();

                CountryDropdownEdit.Click();
                SelectElement selectCountry = new SelectElement(CountryDropdownEdit);
                selectCountry.SelectByText(CountryOfCollege);

                DegreeInput.Click(); DegreeInput.Clear();

                UpdateButton.Click();
            }
            public void EditEducation(string CollegeName, string Degree)
            {
                EditButton(CollegeName).Click();
                //edit degree
                DegreeInput.Click(); DegreeInput.Clear();
                DegreeInput.SendKeys(Degree);
                UpdateButton.Click();
            }
            public bool IsEducationUpdated(string Degree)
            {
                try
                {
                    var educationElement = DegreeElement(Degree);
                    return educationElement != null;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }
            public void DeleteEducation(string collegeName)
            {
                DeleteButton(collegeName).Click();

            }
            public bool IsEducationDeleted(string collegeName)
            {
                try
                {

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    wait.Until(driver => driver.FindElements(By.XPath($"//td[text()='{collegeName}']")).Count == 0);

                    Console.WriteLine($"Education '{collegeName}' was not found in the list.");
                    return true;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Education '{collegeName}' was found in the list.");
                    return false;
                }
            }

            public static List<string> GetAllEducationEntries()
            {
                List<string> educationEntries = new List<string>();

                // Locate the education elements in the list
                var eduEntryElements = driver.FindElements(By.XPath("//th[contains(text(),'University')]/ancestor::table//tbody/tr/td[2]"));

                // Extract the text (college name) from each element and add it to the list
                foreach (var element in eduEntryElements)
                {
                    educationEntries.Add(element.Text);
                }

                return educationEntries;
            }
            public void ClearEducationTestData(string collegeName)
            {
                try
                {

                    DeleteButton(collegeName).Click();

                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"No delete button found for the education: {collegeName}.");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Timeout while waiting for delete button for education: {collegeName}.");
                }
                catch (WebDriverException ex)
                {
                    Console.WriteLine($"Error interacting with delete button: {ex.Message}");
                }
            }
            public void ClearAllEducation()
            {
                // Retrieve the list of all education currently present
                var allEducations = GetAllEducationEntries();

                // Loop through the list and delete each EDUCATION
                foreach (var educations in allEducations)
                {
                    ClearEducationTestData(educations);
                }
            }
            public void CleanupAddedEducationEntries()
            {
                foreach (var collegeName in addedEducationEntries)
                {

                    try
                    {

                        DeleteButton(collegeName).Click();
                        new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                            .Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath($"//tr[contains(., '{collegeName}')]")));
                    }
                    catch (NoSuchElementException)
                    {

                        Console.WriteLine($"College '{collegeName}' not found for deletion. It may not have been added.");
                    }
                }
            }


        
    }
}


