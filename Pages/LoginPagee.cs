
using OpenQA.Selenium;
using Serilog;

namespace SauceDemoAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly ILogger _logger;

        // CSS Selectors
        private readonly string _usernameInputSelector = "#user-name";
        private readonly string _passwordInputSelector = "#password";
        private readonly string _loginButtonSelector = "#login-button";
        private readonly string _errorMessageSelector = "[data-test='error']";

        public LoginPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public LoginPage Navigate()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            _logger.Information("Navigated to SauceDemo login page");
            return this;
        }

        public LoginPage EnterUsername(string username)
        {
            _driver.FindElement(By.CssSelector(_usernameInputSelector)).SendKeys(username);
            _logger.Information($"Entered username: {username}");
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            _driver.FindElement(By.CssSelector(_passwordInputSelector)).SendKeys(password);
            _logger.Information("Entered password");
            return this;
        }

        public LoginPage ClearUsername()
        {
            _driver.FindElement(By.CssSelector(_usernameInputSelector)).Clear();
            _logger.Information("Cleared username field");
            return this;
        }

        public LoginPage ClearPassword()
        {
            _driver.FindElement(By.CssSelector(_passwordInputSelector)).Clear();
            _logger.Information("Cleared password field");
            return this;
        }

        public void ClickLogin()
        {
            _driver.FindElement(By.CssSelector(_loginButtonSelector)).Click();
            _logger.Information("Clicked login button");
        }

        public string GetErrorMessage()
        {
            var errorElement = _driver.FindElement(By.CssSelector(_errorMessageSelector));
            var errorMessage = errorElement.Text;
            _logger.Information($"Error message displayed: {errorMessage}");
            return errorMessage;
        }

        public DashboardPage LoginSuccessfully(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
            return new DashboardPage(_driver, _logger);
        }
    }
}
