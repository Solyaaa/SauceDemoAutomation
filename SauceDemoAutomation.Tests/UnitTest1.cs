using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xunit;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoAutomation.Drivers;
using SauceDemoAutomation.Pages;
using Serilog;

namespace SauceDemoAutomation.Tests;

public class LoginTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly DashboardPage _dashboardPage;
    private readonly LoginPage2 _loginPage;
    private readonly ILogger _logger;
    private readonly WebDriverWait _wait;

    public LoginTests()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/saucedemo_tests.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var factory = new WebDriverFactory(_logger);
        _driver = factory.CreateDriver(BrowserType.Chrome);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        _loginPage = new LoginPage2(_driver, _logger);
        _dashboardPage = new DashboardPage(_driver, _logger);
    }

    public void Dispose()
    {
        _driver?.Quit();
        _logger?.Information("WebDriver closed");
    }

    [Theory]
    [MemberData(nameof(BrowserData))]
    public void UC1_EmptyCredentials_ShouldShowUsernameError(BrowserType browserType)
    {

    var factory = new WebDriverFactory(_logger);
    var driver = factory.CreateDriver(browserType);
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));


    try
    {
        var stopwatch = Stopwatch.StartNew();

        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        _logger.Information("Navigated to SauceDemo login page");

        var usernameField = driver.FindElement(By.CssSelector("#user-name"));
        var passwordField = driver.FindElement(By.CssSelector("#password"));
        var loginButton = driver.FindElement(By.CssSelector("#login-button"));


        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].value = '';", usernameField);
        js.ExecuteScript("arguments[0].value = '';", passwordField);


        _logger.Information($"Username field value: '{usernameField.GetAttribute("value")}'");
        _logger.Information($"Password field value: '{passwordField.GetAttribute("value")}'");


        loginButton.Click();
        _logger.Information("Clicked login button with empty fields");


        var errorElement = wait.Until(d => d.FindElement(By.CssSelector("[data-test='error']")));
        var errorMessage = errorElement.Text;


        _logger.Information($"Actual error message: '{errorMessage}'");

        errorMessage.Should().Contain("Epic sadface: Username is required");

        stopwatch.Stop();
        _logger.Information($"Test UC1 executed in {stopwatch.ElapsedMilliseconds} ms");
    }
    finally
    {
        Thread.Sleep(1000);
        driver.Quit();
        _logger.Information("Browser closed");
    }
}
    [Theory]
    [MemberData(nameof(BrowserData))]
    public void UC2_MissingPassword_ShouldShowPasswordError(BrowserType browserType)
    {
        var factory = new WebDriverFactory(_logger);
        using var driver = factory.CreateDriver(browserType); //
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        var loginPage = new LoginPage2(driver, _logger);

        var stopwatch = Stopwatch.StartNew();
        loginPage.Navigate();
        Thread.Sleep(1000);

        loginPage.EnterUsername("testuser");
        Thread.Sleep(1000);
        loginPage.ClearPassword();
        loginPage.ClickLogin();
        Thread.Sleep(1000);

        var errorElement = wait.Until(d => d.FindElement(By.CssSelector("[data-test='error']")));
        var errorMessage = errorElement.Text;

        _logger.Information($"Actual error message: {errorMessage}");
        errorMessage.Should().Contain("Password is required");

        stopwatch.Stop();
        _logger.Information($"Test UC2 executed in {stopwatch.ElapsedMilliseconds} ms using {browserType}");

        Thread.Sleep(3000);
    }


    [Theory]
    [MemberData(nameof(ValidCredentialsData))]
    public void UC3_ValidCredentials_ShouldLoginSuccessfully(string username, string password, BrowserType browserType)
    {
        var factory = new WebDriverFactory(_logger);
        using var driver = factory.CreateDriver(browserType);
        var loginPage = new LoginPage2(driver, _logger);
        var dashboardPage = new DashboardPage(driver, _logger);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var stopwatch = Stopwatch.StartNew();
        loginPage.Navigate();
        Thread.Sleep(1000);

        loginPage.EnterUsername(username);
        Thread.Sleep(1000);
        loginPage.EnterPassword(password);
        Thread.Sleep(1000);

        loginPage.ClickLogin();
        Thread.Sleep(2000);

        _logger.Information($"Current page HTML: {driver.PageSource}");

        var logo = wait.Until(d => d.FindElement(By.CssSelector(".app_logo")));
        _logger.Information("Dashboard loaded successfully");

        var title = logo.Text;
        title.Should().Be("Swag Labs");

        stopwatch.Stop();
        _logger.Information($"Test UC3 executed in {stopwatch.ElapsedMilliseconds} ms using {browserType}");

        Thread.Sleep(3000);
    }





    public static IEnumerable<object[]> BrowserData()
    {
        yield return new object[] { BrowserType.Chrome };
        yield return new object[] { BrowserType.Firefox };
    }

    public static IEnumerable<object[]> ValidCredentialsData()
    {
        var browsers = new[] { BrowserType.Chrome, BrowserType.Firefox };
        var usernames = new[] { "standard_user", "problem_user", "performance_glitch_user" };

        foreach (var browser in browsers)
        {
            foreach (var username in usernames)
            {
                yield return new object[] { username, "secret_sauce", browser };
            }
        }
    }
}