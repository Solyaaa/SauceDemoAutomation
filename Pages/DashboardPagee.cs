
using OpenQA.Selenium;
using Serilog;

namespace SauceDemoAutomation.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly ILogger _logger;

        // CSS Selectors
        private readonly string _titleSelector = ".app_logo";

        public DashboardPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public string GetTitle()
        {
            var title = _driver.FindElement(By.CssSelector(_titleSelector)).Text;
            _logger.Information($"Dashboard title: {title}");
            return title;
        }
    }
}
