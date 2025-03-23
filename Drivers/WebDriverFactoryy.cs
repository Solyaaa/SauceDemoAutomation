
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Serilog;

namespace SauceDemoAutomation.Drivers
{
    public enum BrowserType
    {
        Chrome,
        Firefox
    }

    public class WebDriverFactory
    {
        private readonly ILogger _logger;

        public WebDriverFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IWebDriver CreateDriver(BrowserType browserType)
        {
            IWebDriver driver;

            switch (browserType)
            {
                case BrowserType.Chrome:
                    driver = CreateChromeDriver();
                    break;
                case BrowserType.Firefox:
                    driver = CreateFirefoxDriver();
                    break;
                default:
                    throw new ArgumentException($"Browser type {browserType} is not supported");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _logger.Information($"Created {browserType} WebDriver instance");
            return driver;
        }

        private IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            // Add any Chrome-specific options here
            return new ChromeDriver(options);
        }

        private IWebDriver CreateFirefoxDriver()
        {
            var options = new FirefoxOptions();
            // Add any Firefox-specific options here
            return new FirefoxDriver(options);
        }
    }
}
