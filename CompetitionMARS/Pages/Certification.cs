using CompetitionMARS.Utilities;
using CompetitionMARS.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Runtime.ConstrainedExecution;

namespace CompetitionMARS.Pages
{
    public class Certification : CommonDriver
    {
        WaitUtils utils = new WaitUtils();
        private WebDriverWait wait;
        public List<string> addedCertificationsEntries = new List<string>();
        private IWebElement CertificationTab => driver.FindElement(By.XPath("//a[contains(text(),'Certifications')]"));
        private IWebElement AddnewCertification => driver.FindElement(By.XPath("//thead/tr[1]/th[4]/div[1]"));
        private IWebElement CertOrAwardInput => driver.FindElement(By.XPath("//input[contains(@placeholder,'Certificate or Award')]"));
        private IWebElement CertificationFrom => driver.FindElement(By.XPath("//input[contains(@name,'certificationFrom')]"));
        private IWebElement CertificationYearDropdown => driver.FindElement(By.XPath("//select[@class='ui fluid dropdown' and @name='certificationYear']"));
        private IWebElement AddCert => driver.FindElement(By.XPath("//input[contains(@value,'Add')]"));
        private IWebElement DeleteButton(string certificateName) => driver.FindElement(By.XPath($"//tbody/tr[td[contains(text(),'{certificateName}')]]/td[4]/span[2]/i[1]"));
        private IWebElement EditButton(string certificateName) => driver.FindElement(By.XPath($"//tr[td[1][contains(text(), '{certificateName}')]]/td[4]/span[1]/i[1]"));
        private IWebElement UpdateButton => driver.FindElement(By.XPath("//input[contains(@value,'Update')]"));
        // private By certifiedFromTemp(string certifiedFromName) => By.XPath($"//td[text()='{certifiedFromName}']");
        private IWebElement CertifiedFromElement(string certifiedFromName) => driver.FindElement(By.XPath($"//td[text()='{certifiedFromName}']"));
        private IWebElement ErrorElement => driver.FindElement(By.XPath("//div[contains(text(),'Please enter Certification Name, Certification Fro')]"));
        //private By entryElementTemp(string certificateName) => By.XPath($"//tr/td[contains(text(), '{certificateName}')]");
        private IWebElement EntryElement(string certificateName) => driver.FindElement(By.XPath($"//tr/td[contains(text(), '{certificateName}')]"));
        //private By certificateElementTemp(string certificateName) => By.XPath($"//tbody/tr[td[contains(text(), '{certificateName}')]]");
        //private IWebElement CertificateElement(string certificateName) => utils.WaitToBeVisible(certificateElementTemp(certificateName), 20);
        private IWebElement CertificateElement(string certificateName) => driver.FindElement(By.XPath($"//tbody/tr[td[contains(text(), '{certificateName}')]]"));
        public CertificationData GetCertificationData(string id)
        {
            // Path to your JSON file
            string jsonFilePath = @"Certification.json";

            // Read the JSON file and deserialize it into a EducationData object
            List<CertificationData> certificationDataList = JsonReader.ReadJsonFile<List<CertificationData>>(jsonFilePath);
            CertificationData cert = certificationDataList.FirstOrDefault(e => e.id == id);
            return cert;

        }
        public void NavigateToCertificationTab()
        {
            CertificationTab.Click();

        }
        public void AddNewCertification(string CertificateOrAward, string CertifiedFrom, int CertificationYear)
        {
            NavigateToCertificationTab();
            AddnewCertification.Click();
            CertOrAwardInput.Click();
            CertOrAwardInput.SendKeys(CertificateOrAward);
            CertificationFrom.Click();
            CertificationFrom.SendKeys(CertifiedFrom);
            CertificationYearDropdown.Click();
            SelectElement selectYearOfCertification = new SelectElement(CertificationYearDropdown);
            selectYearOfCertification.SelectByValue(CertificationYear.ToString());
            AddCert.Click();
            addedCertificationsEntries.Add(CertificateOrAward);

        }
        public bool IsCertificationAdded(string certificateName)
        {
            try
            {
                // Look for the added certification using XPath 
                // var certificateElement = driver.FindElement(By.XPath($"//tbody/tr[td[contains(text(), '{certificateName}')]]"));
                var certificateEle= CertificateElement(certificateName);
                return certificateEle != null; 
            }
            catch (NoSuchElementException)
            {
                return false; 
            }
        }

        public void AddNewCertification_missingField(string CertificateOrAward, int CertificationYear)
        {
            NavigateToCertificationTab();
            AddnewCertification.Click();
            CertOrAwardInput.Click();
            CertOrAwardInput.SendKeys(CertificateOrAward);

            CertificationYearDropdown.Click();
            SelectElement selectYearOfCertification = new SelectElement(CertificationYearDropdown);
            selectYearOfCertification.SelectByValue(CertificationYear.ToString());
            AddCert.Click();

        }
        public bool IsCertificationEntryPresent(string certificateName)
        {
            try
            {
               // var entry = driver.FindElement(By.XPath($"//tr/td[contains(text(), '{certificateName}')]"));
                var entry = EntryElement(certificateName);
                return entry != null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public string GetErrorMessage()
        {
            try
            {
                // Locate the error message element 
                //var errorElement = driver.FindElement(By.XPath("//div[contains(text(),'Please enter Certification Name, Certification Fro')]"));
                var error = ErrorElement;
                return error.Text; 
            }
            catch (NoSuchElementException)
            {
                return string.Empty; 
            }
        }
        public static List<string> GetAllCertificationEntries()
        {
            List<string> certificationEntries = new List<string>();

            // Locate the certification elements in the list
            var certEntryElements = driver.FindElements(By.XPath("//th[contains(text(),'Certificate')]/ancestor::table//tbody/tr/td[1]"));

            // Extract the text (certificate name) from each element and add it to the list
            foreach (var element in certEntryElements)
            {
                certificationEntries.Add(element.Text);
            }

            return certificationEntries;
        }
        public void ClearCertificationTestData(string certificateName)
        {
            try
            {

                DeleteButton(certificateName).Click();

            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"No delete button found for the certification: {certificateName}.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Timeout while waiting for delete button for certification: {certificateName}.");
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"Error interacting with delete button: {ex.Message}");
            }
        }
        public void ClearAllCertification()
        {
            // Retrieve the list of all certification currently present
            var allCertifications = GetAllCertificationEntries();

            // Loop through the list and delete each certification
            foreach (var certifications in allCertifications)
            {
                ClearCertificationTestData(certifications);
            }
        }
        public void EditCertification(string CertificateName, string CertifiedFrom)
        {
            EditButton(CertificateName).Click();
            //edit certified from
            CertificationFrom.Click(); CertificationFrom.Clear();
            CertificationFrom.SendKeys(CertifiedFrom);
            //Click on update
            UpdateButton.Click();
        }
        public bool IsCertificationUpdated(string CertifiedFrom)
        {
            try
            {
                var certifiedElement = CertifiedFromElement(CertifiedFrom);
                return certifiedElement != null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public void EditCertification_missing(string CertificateName, string CertifiedFrom, int CertificationYear)
        {
            EditButton(CertificateName).Click();

            CertificationFrom.Click(); CertificationFrom.Clear();
            CertificationYearDropdown.Click();
            SelectElement selectYearOfCertification = new SelectElement(CertificationYearDropdown);
            selectYearOfCertification.SelectByValue(CertificationYear.ToString());

            UpdateButton.Click();
        }
        public void DeleteCertification(string certificateName)
        {
            DeleteButton(certificateName).Click();

            

        }
        public bool IsCertificationDeleted(string certificateName)
        {
            try
            {
                
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(driver => driver.FindElements(By.XPath($"//td[text()='{certificateName}']")).Count == 0);

                Console.WriteLine($"Certification '{certificateName}' was not found in the list.");
                return true;  
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Certification '{certificateName}' was found in the list.");
                return false;  
            }
        }
        
        public void CleanupAddedCertificationEntries()
        {
            foreach (var certificateName in addedCertificationsEntries)
            {
                //Locate the added certification entry by CertificateName and delete it
                try
                {
                   // var deleteButton = driver.FindElement(By.XPath($"//tbody/tr[td[contains(text(),'{certificateName}')]]/td[4]/span[2]/i[1]"));
                    DeleteButton(certificateName).Click();
                    new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                        .Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath($"//tr[contains(., '{certificateName}')]")));
                }
                catch (NoSuchElementException)
                {

                    Console.WriteLine($"College '{certificateName}' not found for deletion. It may not have been added.");
                }
            }

        }
    }
}

