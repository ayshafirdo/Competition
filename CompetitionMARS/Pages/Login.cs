using CompetitionMARS.Models;
using CompetitionMARS.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionMARS.Pages
{
    public class LoginActions : CommonDriver

    {

        public LoginData GetLoginData() {
            // Path to your JSON file
            string jsonFilePath = @"C:\\Users\\aysha\\Repositories\\Competition\\CompetitionMARS\\JsonFiles\\LoginData.json";

            // Read the JSON file and deserialize it into a LoginData object
            LoginData loginData = JsonReader.ReadJsonFile<LoginData>(jsonFilePath);

            return loginData;
        } 
        public void Login(String email, String password)

        {


            try
            {

                //Click on sign in button
                IWebElement loginButton = driver.FindElement(By.XPath("//a[contains(text(),'Sign In')]"));
                loginButton.Click();
                Thread.Sleep(2000);
                IWebElement emailTextBox = driver.FindElement(By.XPath("//input[contains(@placeholder,\"Email address\")]"));
                emailTextBox.Click(); emailTextBox.SendKeys(email);
                IWebElement passwordTextBox = driver.FindElement(By.XPath("//input[contains(@placeholder,\"Password\")]"));
                passwordTextBox.Click(); passwordTextBox.SendKeys(password);
                IWebElement loginButton2 = driver.FindElement(By.XPath("//button[contains(text(),'Login')]"));
                loginButton2.Click();
                Thread.Sleep(5000);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
        /*Locators
        private IWebElement SigninButton => driver.FindElement(By.XPath("//a[contains(text(),'Sign In')]"));
        private IWebElement EmailTextBox => driver.FindElement(By.XPath("//input[contains(@placeholder,\"Email address\")]"));
        private IWebElement PasswordTextBox =>driver.FindElement( By.XPath("//input[contains(@placeholder,\"Password\")]"));
        private IWebElement LoginButton => driver.FindElement(By.XPath("//button[contains(text(),'Login')]"));
        public void Login(String email, String password)

        {


            try
            {

                //Click on sign in button
               
                SigninButton.Click();

                EmailTextBox.Click(); EmailTextBox.SendKeys(email);
                
                PasswordTextBox.Click(); PasswordTextBox.SendKeys(password);
                
                LoginButton.Click();
               

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

    }
}*/
