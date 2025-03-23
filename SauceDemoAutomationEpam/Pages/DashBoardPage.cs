using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace SauceDemoAutomation.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly ILogger _logger;


        private readonly string _titleSelector = ".app_logo";

        public DashboardPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public string GetTitle()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var logo = wait.Until(d => d.FindElement(By.CssSelector(".app_logo")));
            return logo.Text;
        }

    }
}