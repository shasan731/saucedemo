
using Xunit;
using SauceDemoTests.Pages;
using FluentAssertions;



namespace SauceDemoTests.Tests
{
    public class SauceDemoTests
    {
        private const string StandardUser = "standard_user";
        private const string LockedOutUser = "locked_out_user";
        private const string PerformanceUser = "performance_glitch_user";
        private const string Password = "secret_sauce";
        
        // Q1 Try login with locked_out_user and verify the error message.
        
        [Fact(DisplayName = "Q1: Locked out user sees proper error")]
        public void LockedOutUser_ShouldSeeErrorMessage()
        {
            using var fixture = new DriverFixture();
            var loginPage = new LoginPage(fixture.Driver);
            loginPage.GoTo();
            loginPage.Login(LockedOutUser, Password);
            var error = loginPage.GetErrorMessage();
            error.ToLowerInvariant().Should().Contain("locked out");
        }

        // Q2 Standard user full purchase of 3 items
        [Fact(DisplayName = "Q2: Standard user purchases 3 items end-to-end")]

        public void StandardUser_CanPurchaseThreeItems()
        {
            using var fixture = new DriverFixture();
            var helper = new ScenarioHelper(fixture.Driver);
            helper.RunStandardUserScenario();
        }

        // Q3 Performance glitch user: reset, sort Z→A, first item purchase
        [Fact(DisplayName = "Q3: Performance glitch user purchases first item (Z→A)")]

        public void PerformanceGlitchUser_CanPurchaseFirstItemSortedDescending()
        {
            using var fixture = new DriverFixture();
            var helper = new ScenarioHelper(fixture.Driver);
            helper.RunPerformanceGlitchUserScenario();
        }

        /// Runs all three scenarios
        [Fact(DisplayName = "All test scenarios")]
        
        public void AllScenarios_CanRunSequentially()
        {
            using var fixture = new DriverFixture();
            var helper = new ScenarioHelper(fixture.Driver);
            helper.RunLockedOutUserScenario();
            helper.RunStandardUserScenario();
            helper.RunPerformanceGlitchUserScenario();
        }
    }
}
