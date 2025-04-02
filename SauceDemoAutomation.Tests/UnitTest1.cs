
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoAutomation.Drivers;
using SauceDemoAutomation.Pages;
using SauceDemoAutomation.Tests.Logging;
namespace SauceDemoAutomation.Tests
{
    public class LoginTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly DashboardPage _dashboardPage;
        private readonly LoginPage2 _loginPage;
        private readonly ITestLogger _testLogger;
        private readonly WebDriverWait _wait;

        public LoginTests()
        {
            _testLogger = new TestLogger();

            var factory = new WebDriverFactory(_testLogger.GetSerilogLogger());
            _driver = factory.CreateDriver(BrowserType.Chrome);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _loginPage = new LoginPage2(_driver, _testLogger);
            _dashboardPage = new DashboardPage(_driver, _testLogger);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _testLogger.LogCloseBrowser();
        }

        [Theory]
        [MemberData(nameof(BrowserData))]
        public void UC1_EmptyCredentials_ShouldShowUsernameError(BrowserType browserType)
        {
            var factory = new WebDriverFactory(_testLogger.GetSerilogLogger());
            var driver = factory.CreateDriver(browserType);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage2(driver, _testLogger);

            try
            {
                var stopwatch = Stopwatch.StartNew();

                loginPage.Navigate();
                loginPage.ClearUsername();
                loginPage.ClearPassword();
                loginPage.ClickLogin();

                var errorMessage = loginPage.GetErrorMessage();
                errorMessage.Should().Contain("Epic sadface: Username is required");

                stopwatch.Stop();
                _testLogger.LogTestExecution("UC1", stopwatch.ElapsedMilliseconds, browserType.ToString());
            }
            finally
            {
                Thread.Sleep(1000);
                driver.Quit();
                _testLogger.LogCloseBrowser();
            }
        }

        [Theory]
        [MemberData(nameof(BrowserData))]
        public void UC2_MissingPassword_ShouldShowPasswordError(BrowserType browserType)
        {
            var factory = new WebDriverFactory(_testLogger.GetSerilogLogger());
            using var driver = factory.CreateDriver(browserType);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage2(driver, _testLogger);

            var stopwatch = Stopwatch.StartNew();

            loginPage.Navigate();
            Thread.Sleep(1000);
            loginPage.EnterUsername("testuser");
            Thread.Sleep(1000);
            loginPage.ClearPassword();
            loginPage.ClickLogin();
            Thread.Sleep(1000);

            var errorMessage = loginPage.GetErrorMessage();
            errorMessage.Should().Contain("Password is required");

            stopwatch.Stop();
            _testLogger.LogTestExecution("UC2", stopwatch.ElapsedMilliseconds, browserType.ToString());

            Thread.Sleep(3000);
        }

        [Theory]
        [MemberData(nameof(ValidCredentialsData))]
        public void UC3_ValidCredentials_ShouldLoginSuccessfully(string username, string password, BrowserType browserType)
        {
            var factory = new WebDriverFactory(_testLogger.GetSerilogLogger()); // Fix this line
            using var driver = factory.CreateDriver(browserType);
            var loginPage = new LoginPage2(driver, _testLogger);
            var dashboardPage = new DashboardPage(driver, _testLogger);
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

            var title = dashboardPage.GetLogoText();
            title.Should().Be("Swag Labs");

            stopwatch.Stop();
            _testLogger.LogTestExecution("UC3", stopwatch.ElapsedMilliseconds, browserType.ToString());

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
}
