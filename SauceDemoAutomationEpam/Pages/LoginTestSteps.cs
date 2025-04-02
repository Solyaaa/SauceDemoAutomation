
using Serilog;
using Serilog.Events;

using System.Diagnostics;


namespace SauceDemoAutomation.Tests.Logging
{

    public class TestLogger : ITestLogger
    {
        private readonly ILogger _logger;
        private readonly string _logFilePath;
        private static TestLogger _instance;

        public TestLogger(string logFileName = "saucedemo_tests.log", LogEventLevel minimumLevel = LogEventLevel.Information)
        {

            Directory.CreateDirectory("logs");

            _logFilePath = $"logs/{logFileName}";

            _logger = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .WriteTo.Console()
                .WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("TestRunId", Guid.NewGuid().ToString())
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .CreateLogger();

            _logger.Information($"TestLogger initialized with log file: {_logFilePath}");
            _instance = this;
        }


        public static TestLogger Instance => _instance ?? new TestLogger();

        public ILogger GetSerilogLogger()
        {
            return _logger;
        }

        public void StartTest(string testName)
        {
            _logger.Information($"========== TEST START: {testName} ==========");
        }

        public void EndTest(string testName, bool passed, long durationMs = 0)
        {
            var status = passed ? "PASSED" : "FAILED";
            _logger.Information($"========== TEST {status}: {testName}, Duration: {durationMs}ms ==========");
        }

        public void LogNavigation(string url)
        {
            _logger.Information($"Navigated to: {url}");
        }

        public void LogInput(string fieldName, string value, bool maskValue = false)
        {
            string displayValue = maskValue ? "********" : value;
            _logger.Information($"Entered {fieldName}: '{displayValue}'");
        }

        public void LogClearField(string fieldName)
        {
            _logger.Information($"Cleared {fieldName} field");
        }

        public void LogButtonClick(string buttonName)
        {
            _logger.Information($"Clicked {buttonName} button");
        }

        public void LogErrorMessage(string errorMessage)
        {
            _logger.Information($"Error message displayed: '{errorMessage}'");
        }

        public void LogPageLoad(string pageName)
        {
            _logger.Information($"{pageName} loaded successfully");
        }

        public void LogPageSource(string pageSource, bool truncate = true)
        {
            if (truncate && pageSource.Length > 1000)
            {
                pageSource = pageSource.Substring(0, 1000) + "... [truncated]";
            }
            _logger.Information($"Current page HTML: {pageSource}");
        }

        public void LogCloseBrowser()
        {
            _logger.Information("Browser closed");
        }

        public void LogTestExecution(string testName, long elapsedMilliseconds, string browserType)
        {
            _logger.Information($"Test {testName} executed in {elapsedMilliseconds} ms using {browserType}");
        }

        public void LogGeneric(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                _logger.Error(ex, message);
            }
            else
            {
                _logger.Error(message);
            }
        }

        public void LogWithContext(string message, params (string Key, object Value)[] contextData)
        {
            var contextLogger = _logger;
            foreach (var (key, value) in contextData)
            {
                contextLogger = contextLogger.ForContext(key, value);
            }
            contextLogger.Information(message);
        }

        public void LogScreenshot(string screenshotFilePath, string description)
        {
            _logger.Information($"Screenshot captured: {description} - Saved to: {screenshotFilePath}");
        }

        public IDisposable BeginScope(string operationName)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.Information($"Operation started: {operationName}");

            return new ScopeDisposable(() =>
            {
                stopwatch.Stop();
                _logger.Information($"Operation completed: {operationName} - Duration: {stopwatch.ElapsedMilliseconds}ms");
            });
        }

        private class ScopeDisposable : IDisposable
        {
            private readonly Action _onDispose;

            public ScopeDisposable(Action onDispose)
            {
                _onDispose = onDispose;
            }

            public void Dispose()
            {
                _onDispose?.Invoke();
            }
        }
    }
}
