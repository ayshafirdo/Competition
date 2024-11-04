using CompetitionMARS.Models;
using CompetitionMARS.Pages;
using CompetitionMARS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionMARS.Tests
{
    public class LoginTest : CommonDriver
    {
        private LoginActions loginPage;
        [SetUp]
        public void SetUp()
        {
            loginPage = new LoginActions();
           
        }

        [Test]
        public void ValidLoginTest()
        {


            //call a method in loginpage which will return logindata
            LoginData loginuser = loginPage.GetLoginData();

            // Perform login using the JSON data
            loginPage.Login(loginuser.Email, loginuser.Password);



            // Assert that login was successful (you can adapt this assertion to fit your application)
            Assert.AreEqual("http://localhost:5000/Account/Profile", driver.Url);
        }
        
    
        [TearDown]
        public void TearDown()
        {
            // Close browser or any cleanup logic
            driver.Quit();
        }
    }
}
    

