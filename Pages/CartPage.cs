using OpenQA.Selenium;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly By _checkoutBtn = By.Id("checkout");

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ProceedToCheckout()
        {
            var btn = _driver.FindElement(_checkoutBtn);
            btn.Click();
            TestDelays.Pause();
        }
    }
}