using OpenQA.Selenium;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class CheckoutStepOnePage
    {
        private readonly IWebDriver _driver;

        public CheckoutStepOnePage(IWebDriver driver) => _driver = driver;

        public void EnterYourInformation(string first, string last, string zip)
        {
            _driver.FindElement(By.Id("first-name")).SendKeys(first);
            TestDelays.Pause();
            _driver.FindElement(By.Id("last-name")).SendKeys(last);
            TestDelays.Pause();
            _driver.FindElement(By.Id("postal-code")).SendKeys(zip);
            TestDelays.Pause();

            _driver.FindElement(By.Id("continue")).Click();
            TestDelays.Pause();
        }
    }
}