
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SauceDemoAutomation.Tests.Logging;

namespace SauceDemoAutomation.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly ITestLogger _logger;
        private readonly WebDriverWait _wait;

        public DashboardPage(IWebDriver driver, ITestLogger logger)
        {
            _driver = driver;
            _logger = logger;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public string GetLogoText()
        {
            var logo = _wait.Until(d => d.FindElement(By.CssSelector(".app_logo")));
            _logger.LogPageLoad("Dashboard");
            return logo.Text;
        }
    }
}
