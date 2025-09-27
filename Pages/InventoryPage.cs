using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoTests.Utils;

namespace SauceDemoTests.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;

        public InventoryPage(IWebDriver driver) { _driver = driver; }

        
        private WebDriverWait Wait(int seconds = 30)
            => new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));


        private void EnsureInventoryLoaded()
        {
            
            Wait().Until(d =>
            {
                try
                {
                    var title = d.FindElement(By.CssSelector("[data-test='title']"));
                    return title.Displayed && title.Text.Trim().Equals("Products", StringComparison.OrdinalIgnoreCase);
                }
                catch (NoSuchElementException) { return false; }
            });

            
            Wait().Until(d => d.FindElements(By.CssSelector(".inventory_item")).Count > 0);
        }


        public void OpenMenu()
        {
            var menuBtn = Wait().Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.Id("react-burger-menu-btn"));
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
            });
            menuBtn.Click();
            TestDelays.Pause();
        }


        public void ResetAppState()
        {
            OpenMenu();

            var resetLink = Wait().Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.Id("reset_sidebar_link"));
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
            });
            resetLink.Click();
            TestDelays.Pause();

            var closeBtn = Wait().Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.Id("react-burger-cross-btn"));
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
            });
            closeBtn.Click();
            TestDelays.Pause();

            EnsureInventoryLoaded();
        }


        public CartPage GoToCart()
        {
            var cartLink = Wait().Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.CssSelector("a.shopping_cart_link[data-test='shopping-cart-link']"));
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
            });
            cartLink.Click();
            TestDelays.Pause();


            Wait().Until(d =>
            {
                var urlOk = d.Url.Contains("cart.html", StringComparison.OrdinalIgnoreCase);
                var titleOk = false;
                try
                {
                    var title = d.FindElement(By.CssSelector("[data-test='title']"));
                    titleOk = title.Displayed && title.Text.Trim().Equals("Your Cart", StringComparison.OrdinalIgnoreCase);
                }
                catch (NoSuchElementException) { /* ignore */ }
                return urlOk || titleOk;
            });

            return new CartPage(_driver);
        }


        public List<string> AddFirstNItemsToCartAndReturnNames(int count)
        {
            EnsureInventoryLoaded();

            var names = new List<string>();
            var items = _driver.FindElements(By.CssSelector(".inventory_item")).ToList();

            for (int i = 0; i < count && i < items.Count; i++)
            {
                var item = items[i];
                var nameEl = item.FindElement(By.ClassName("inventory_item_name"));
                names.Add(nameEl.Text);

                var button = item.FindElement(By.CssSelector("button.btn_inventory"));

                
                try { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", button); }
                catch { /*l*/ }

                button.Click();
                TestDelays.Pause();

                
                Wait().Until(_ =>
                {
                    try { return button.Text.Trim().Equals("Remove", StringComparison.OrdinalIgnoreCase); }
                    catch (StaleElementReferenceException) { return true; }
                });
            }

            
            Wait().Until(d =>
            {
                try
                {
                    var badge = d.FindElement(By.CssSelector(".shopping_cart_badge"));
                    return badge.Displayed && int.TryParse(badge.Text.Trim(), out var n) && n >= names.Count;
                }
                catch (NoSuchElementException) { return false; }
            });

            return names;
        }


        public string AddFirstItemToCartAndReturnName()
        {
            EnsureInventoryLoaded();

            var items = _driver.FindElements(By.CssSelector(".inventory_item")).ToList();
            if (!items.Any())
                throw new InvalidOperationException("No inventory items found on the page.");

            var first = items.First();
            var name = first.FindElement(By.ClassName("inventory_item_name")).Text;
            var button = first.FindElement(By.CssSelector("button.btn_inventory"));

            try { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", button); }
            catch { /*  */ }

            button.Click();
            TestDelays.Pause();

            Wait().Until(_ =>
            {
                try { return button.Text.Trim().Equals("Remove", StringComparison.OrdinalIgnoreCase); }
                catch (StaleElementReferenceException) { return true; }
            });

            Wait().Until(d =>
            {
                try
                {
                    var badge = d.FindElement(By.CssSelector(".shopping_cart_badge"));
                    return badge.Displayed && int.TryParse(badge.Text.Trim(), out var n) && n >= 1;
                }
                catch (NoSuchElementException) { return false; }
            });

            return name;
        }


        public List<string> GetNamesOfFirstNItems(int count)
        {
            EnsureInventoryLoaded();

            var names = new List<string>();
            var items = _driver.FindElements(By.CssSelector(".inventory_item")).ToList();
            for (int i = 0; i < count && i < items.Count; i++)
            {
                names.Add(items[i].FindElement(By.ClassName("inventory_item_name")).Text);
            }
            return names;
        }
        
        public void SortByNameDescending()
        {
            EnsureInventoryLoaded();

            var selectEl = Wait().Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.CssSelector("select[data-test='product-sort-container']"));
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
            });

            var sortSelect = new SelectElement(selectEl);
            sortSelect.SelectByValue("za");
            TestDelays.Pause();

            Wait().Until(d =>
            {
                try
                {
                    var active = d.FindElement(By.CssSelector("span.select_container .active_option[data-test='active-option']"));
                    return active.Displayed && active.Text.Trim().Contains("Z to A", StringComparison.OrdinalIgnoreCase);
                }
                catch (NoSuchElementException) { return false; }
            });

            EnsureInventoryLoaded();
        }
    }
}
