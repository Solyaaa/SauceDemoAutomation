
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace SauceDemoAutomation.Pages
{
    public class LoginPage2
    {
        private readonly IWebDriver _driver;
        private readonly ILogger _logger;
        private readonly WebDriverWait _wait;


    private readonly string _usernameInputSelector = "#user-name";
    private readonly string _passwordInputSelector = "#password";
    private readonly string _loginButtonSelector = "#login-button";
    private readonly string _errorMessageSelector = "[data-test='error']";

    public LoginPage2(IWebDriver driver, ILogger logger)
    {
        _driver = driver;
        _logger = logger;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    public LoginPage2 Navigate()
    {
        _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        _logger.Information("Navigated to SauceDemo login page");
        return this;
    }

    public LoginPage2 EnterUsername(string username)
    {
        var usernameField = _driver.FindElement(By.CssSelector(_usernameInputSelector));
        usernameField.Clear();

        usernameField.SendKeys(username);
        _logger.Information($"Entered username: {username}");
        return this;
    }

    public LoginPage2 EnterUsernameCharByChar(string username)
    {
        var usernameField = _driver.FindElement(By.CssSelector(_usernameInputSelector));
        usernameField.Clear();

        foreach (char c in username)
        {
            usernameField.SendKeys(c.ToString());
            Thread.Sleep(150);
        }

        _logger.Information($"Entered username character by character: {username}");
        return this;
    }

    public LoginPage2 EnterPassword(string password)
    {
        var passwordField = _driver.FindElement(By.CssSelector(_passwordInputSelector));
        passwordField.Clear();
        passwordField.SendKeys(password);
        _logger.Information("Entered password");
        return this;
    }

    public LoginPage2 EnterPasswordCharByChar(string password)
    {
        var passwordField = _driver.FindElement(By.CssSelector(_passwordInputSelector));
        passwordField.Clear();

        foreach (char c in password)
        {
            passwordField.SendKeys(c.ToString());
            Thread.Sleep(150);
        }

        _logger.Information($"Entered password character by character");
        return this;
    }

    public LoginPage2 ClearUsername()
    {
        var usernameField = _driver.FindElement(By.CssSelector(_usernameInputSelector));
        usernameField.Clear();
        Thread.Sleep(500);
        usernameField.SendKeys(Keys.Backspace);

        _logger.Information("Cleared username field");
        return this;
    }

    public LoginPage2 ClearPassword()
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
        try
        {
            var errorElement = _wait.Until(d => d.FindElement(By.CssSelector(_errorMessageSelector)));
            var errorMessage = errorElement.Text;
            _logger.Information($"Error message displayed: {errorMessage}");
            return errorMessage;
        }
        catch (WebDriverTimeoutException)
        {
            _logger.Warning("No error message was displayed within the timeout period");
            return string.Empty;
        }
    }

    public bool IsErrorDisplayed()
    {
        try
        {
            var errorElement = _wait.Until(d => d.FindElement(By.CssSelector(_errorMessageSelector)));
            return errorElement.Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public DashboardPage LoginSuccessfully(string username, string password)
    {
        EnterUsername(username);
        Thread.Sleep(1000);
        EnterPassword(password);
        Thread.Sleep(1000);
        ClickLogin();
        return new DashboardPage(_driver, _logger);
    }

    public DashboardPage LoginSuccessfullySlowly(string username, string password)
    {
        EnterUsernameCharByChar(username);
        Thread.Sleep(1000);
        EnterPasswordCharByChar(password);
        Thread.Sleep(1000);
        ClickLogin();
        return new DashboardPage(_driver, _logger);
    }

    public void WaitForPageToLoad()
    {
        _wait.Until(d => d.FindElement(By.CssSelector(_usernameInputSelector)).Displayed);
        _logger.Information("Login page loaded completely");
    }
}


}
