using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace SnipeIt.Playwright.Tests.Tests;

[TestFixture]
public class BaseTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = Data.TestData.BaseUrl,
        ViewportSize = new() { Width = 1920, Height = 1080 },
        IgnoreHTTPSErrors = true
    };

    [SetUp]
    public async Task Setup() => await Page.GotoAsync("about:blank");


    [TearDown]
    public async Task Teardown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            Console.WriteLine("TEST FAILED → Browser stays open. Close manually.");
            await Page.PauseAsync();
        } else
        {
            Console.WriteLine("TEST PASSED – Closing browser");
        }
    }
}