using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class CheckoutStepTwoPage
    {
        private readonly IWebDriver _driver;

        public CheckoutStepTwoPage(IWebDriver driver) => _driver = driver;

        public List<string> GetProductNames()
        {
            return _driver.FindElements(By.CssSelector(".cart_item .inventory_item_name"))
                .Select(e => e.Text).ToList();
        }

        public void VerifyTotalPrice()
        {
            
            var itemTotal = _driver.FindElement(By.CssSelector(".summary_subtotal_label")).Text;
            var tax = _driver.FindElement(By.CssSelector(".summary_tax_label")).Text;
            var total = _driver.FindElement(By.CssSelector(".summary_total_label")).Text;

            decimal Parse(string s)
            {
                var num = new string(s.Where(ch => char.IsDigit(ch) || ch == '.' || ch == '-').ToArray());
                return decimal.Parse(num, CultureInfo.InvariantCulture);
            }

            var it = Parse(itemTotal);
            var tx = Parse(tax);
            var tt = Parse(total);

            if (Math.Round(it + tx, 2) != Math.Round(tt, 2))
                throw new InvalidOperationException($"Totals mismatch: itemTotal({it}) + tax({tx}) != total({tt})");
        }

        public CheckoutCompletePage Finish()
        {
            _driver.FindElement(By.Id("finish")).Click();
            TestDelays.Pause();
            return new CheckoutCompletePage(_driver);
        }
    }
}