
using Serilog;

namespace SauceDemoAutomation.Tests.Logging;

public interface ITestLogger
{

    ILogger GetSerilogLogger();

    void StartTest(string testName);


    void EndTest(string testName, bool passed, long durationMs = 0);


    void LogNavigation(string url);


    void LogInput(string fieldName, string value, bool maskValue = false);


    void LogClearField(string fieldName);


    void LogButtonClick(string buttonName);


    void LogErrorMessage(string errorMessage);


    void LogPageLoad(string pageName);


    void LogPageSource(string pageSource, bool truncate = true);


    void LogCloseBrowser();


    void LogTestExecution(string testName, long elapsedMilliseconds, string browserType);


    void LogGeneric(string message);


    void LogWarning(string message);


    void LogError(string message, Exception ex = null);


    void LogWithContext(string message, params (string Key, object Value)[] contextData);


    void LogScreenshot(string screenshotFilePath, string description);


    IDisposable BeginScope(string operationName);
}