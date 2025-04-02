
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SauceDemoAutomation.Tests.Logging;

namespace SauceDemoAutomation.Pages
{
    public class LoginPage2
    {
        private readonly IWebDriver _driver;
        private readonly ITestLogger _logger;
        private readonly WebDriverWait _wait;

        public LoginPage2(IWebDriver driver, ITestLogger logger)
        {
            _driver = driver;
            _logger = logger;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            _logger.LogNavigation("SauceDemo login page");
        }

        public void EnterUsername(string username)
        {
            var usernameField = _driver.FindElement(By.CssSelector("#user-name"));
            usernameField.Clear();
            usernameField.SendKeys(username);
            _logger.LogInput("username", username);
        }

        public void ClearUsername()
        {
            var usernameField = _driver.FindElement(By.CssSelector("#user-name"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].value = '';", usernameField);
            _logger.LogClearField("username");
        }

        public void EnterPassword(string password)
        {
            var passwordField = _driver.FindElement(By.CssSelector("#password"));
            passwordField.Clear();
            passwordField.SendKeys(password);
            _logger.LogInput("password", password, true);
        }

        public void ClearPassword()
        {
            var passwordField = _driver.FindElement(By.CssSelector("#password"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].value = '';", passwordField);
            _logger.LogClearField("password");
        }

        public void ClickLogin()
        {
            var loginButton = _driver.FindElement(By.CssSelector("#login-button"));
            loginButton.Click();
            _logger.LogButtonClick("login");
        }

        public string GetErrorMessage()
        {
            var errorElement = _wait.Until(d => d.FindElement(By.CssSelector("[data-test='error']")));
            var errorMessage = errorElement.Text;
            _logger.LogErrorMessage(errorMessage);
            return errorMessage;
        }
    }
}
