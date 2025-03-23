# SauceDemoAutomation

# SauceDemo Automation Tests

##  Description
Automated tests for the [SauceDemo](https://www.saucedemo.com/) website, verifying the login functionality.

##  Test Cases:
1. **UC-1:** Login without username and password → Check error message "Username is required".
2. **UC-2:** Login with only username → Check error message "Password is required".
3. **UC-3:** Login with valid credentials → Verify redirection to "Swag Labs" dashboard.

##  Technologies:
- **Selenium WebDriver** (Browser automation)
- **xUnit** (Test runner)
- **FluentAssertions** (Assertions for validation)
- **Serilog** (Logging to console and file)
- **CSS Selectors** (Element locators)
- **Factory Method** (WebDriver initialization)

## Running Tests

### Run tests in standard mode:
```sh
dotnet test
```

##  Project Structure
- `/Pages/` - Page object models (LoginPage, DashboardPage)
- `/Drivers/` - WebDriver Factory (browser instance creation)
- `/Tests/` - Test scenarios (LoginTests.cs)
- `SauceDemoAutomation.Tests.csproj` - Test project configuration file

##  Environment Setup
1. Install **.NET SDK 8.0+**
2. Install **Chrome** and **Firefox** browsers
3. Install Selenium WebDriver:
```sh
dotnet add package Selenium.WebDriver
```
4. Install FluentAssertions:
```sh
dotnet add package FluentAssertions
```
5. Install Serilog:
```sh
dotnet add package Serilog
```

 **The project is ready to run!** If you have any questions, feel free to ask. 

