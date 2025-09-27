using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemoTests
{
    public class DriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }

        public DriverFixture()
        { 
            var opts = new ChromeOptions();
            
            opts.AddArgument("--incognito");
            opts.AddArgument("--start-maximized");
            opts.AddArgument("--no-first-run");
            opts.AddArgument("--no-default-browser-check");
            opts.AddArgument("--disable-infobars");
            opts.AddArgument("--disable-notifications");
            opts.AddArgument("--disable-extensions");
            
            opts.AddExcludedArgument("enable-automation");
            opts.AddAdditionalOption("useAutomationExtension", false);
            opts.AddUserProfilePreference("credentials_enable_service", false);
            opts.AddUserProfilePreference("profile.password_manager_enabled", false);
            opts.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2); // block

            Driver = new ChromeDriver(opts);
            
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        public void Dispose()
        {
            try { Driver.Quit(); } catch (Exception) { /* b */ }
            try { Driver.Dispose(); } catch (Exception) { /* b */ }
        }
    }
}
