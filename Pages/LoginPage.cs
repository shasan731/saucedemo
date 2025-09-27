using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver) => _driver = driver;

        public void GoTo()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            
            new WebDriverWait(_driver, TimeSpan.FromSeconds(30))
                .Until(d => d.FindElement(By.Id("user-name")).Displayed);
            
        }

        public void Login(string user, string pass)
        {
            _driver.FindElement(By.Id("user-name")).Clear();
            _driver.FindElement(By.Id("user-name")).SendKeys(user);
            TestDelays.Pause();

            _driver.FindElement(By.Id("password")).Clear();
            _driver.FindElement(By.Id("password")).SendKeys(pass);
            TestDelays.Pause();

            _driver.FindElement(By.Id("login-button")).Click();
            TestDelays.Pause();
            
        }

        public string GetErrorMessage()
        {
            var el = _driver.FindElement(By.CssSelector("[data-test='error']"));
            return el.Text;
        }


    }
}
