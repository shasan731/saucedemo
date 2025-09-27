using OpenQA.Selenium;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class CheckoutCompletePage
    {
        private readonly IWebDriver _driver;

        public CheckoutCompletePage(IWebDriver driver) => _driver = driver;

        public bool IsOrderComplete()
        {
            var header = _driver.FindElement(By.ClassName("complete-header"));
            return header.Displayed && header.Text.Trim().ToLowerInvariant().Contains("thank you");
        }

        public void ResetAppStateAndLogout()
        {
            
            _driver.FindElement(By.Id("back-to-products")).Click();
            TestDelays.Pause();

            // open menu, reset, logout
            _driver.FindElement(By.Id("react-burger-menu-btn")).Click();
            TestDelays.Pause();

            _driver.FindElement(By.Id("reset_sidebar_link")).Click();
            TestDelays.Pause();

            _driver.FindElement(By.Id("logout_sidebar_link")).Click();
            TestDelays.Pause();
        }
    }
}