// Utils/WaitHelpers.cs
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SauceDemoTests.Utils
{
    public static class WaitHelpers
    {
        public static IWebElement WaitForVisible(IWebDriver driver, By by, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public static void ClickWhenVisible(IWebDriver driver, By by, int seconds = 10)
        {
            var el = WaitForVisible(driver, by, seconds);
            el.Click();
        }
    }
}