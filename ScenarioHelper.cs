using System;
using System.Collections.Generic;
using FluentAssertions;
using SauceDemoTests.Pages;
using OpenQA.Selenium;

namespace SauceDemoTests
{

    public class ScenarioHelper
    {
        private readonly IWebDriver _driver;

        public ScenarioHelper(IWebDriver driver)
        {
            _driver = driver;
        }
        
        public void RunLockedOutUserScenario()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.GoTo();
            loginPage.Login("locked_out_user", "secret_sauce");
            var errorMessage = loginPage.GetErrorMessage();


            errorMessage.ToLowerInvariant().Should().Contain("locked out");
        }


        public void RunStandardUserScenario()
        {
            // Login
            var loginPage = new LoginPage(_driver);
            loginPage.GoTo();
            loginPage.Login("standard_user", "secret_sauce");

            // Inventory interactions
            var inventoryPage = new InventoryPage(_driver);
            inventoryPage.ResetAppState();
            var selectedNames = inventoryPage.AddFirstNItemsToCartAndReturnNames(3);

            // Proceed to cart and checkout
            var cartPage = inventoryPage.GoToCart();


            cartPage.ProceedToCheckout();
            var stepOne = new CheckoutStepOnePage(_driver);

            // Fill in Step One
            stepOne.EnterYourInformation("Test", "User", "12345");

            // Step Two: verify names & totals, finish
            var stepTwo = new CheckoutStepTwoPage(_driver);
            var namesOnPage = stepTwo.GetProductNames();
            namesOnPage.Should().BeEquivalentTo(selectedNames, options => options.WithStrictOrdering());
            stepTwo.VerifyTotalPrice();

            var completePage = stepTwo.Finish();
            completePage.IsOrderComplete().Should().BeTrue();

            // Reset and logout from the complete page (uses hamburger menu)
            completePage.ResetAppStateAndLogout();
        }


        public void RunPerformanceGlitchUserScenario()
        {
            // Login
            var loginPage = new LoginPage(_driver);
            loginPage.GoTo();
            loginPage.Login("performance_glitch_user", "secret_sauce");

            // Inventory interactions
            var inventoryPage = new InventoryPage(_driver);
            inventoryPage.ResetAppState();
            inventoryPage.SortByNameDescending();
            var selectedName = inventoryPage.AddFirstItemToCartAndReturnName();

            // Proceed to cart and checkout
            var cartPage = inventoryPage.GoToCart();
            
            cartPage.ProceedToCheckout();
            var stepOne = new CheckoutStepOnePage(_driver);

            // Fill in Step One
            stepOne.EnterYourInformation("Test", "User", "12345");

            // Step Two: verify single item + totals, finish
            var stepTwo = new CheckoutStepTwoPage(_driver);
            var namesOnPage = stepTwo.GetProductNames();
            namesOnPage.Should().ContainSingle().Which.Should().Be(selectedName);
            stepTwo.VerifyTotalPrice();

            var completePage = stepTwo.Finish();
            completePage.IsOrderComplete().Should().BeTrue();

            // Reset and logout
            completePage.ResetAppStateAndLogout();
        }
    }
}
