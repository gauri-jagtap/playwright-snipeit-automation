using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static Microsoft.Playwright.Assertions;   // ← THIS IS THE CORRECT LINE

namespace SnipeIt.Playwright.Tests.Pages;

public class BasePage
{
    protected readonly IPage Page;
    protected readonly string BaseUrl = Data.TestData.BaseUrl;

    public BasePage(IPage page) => Page = page;

    public async Task GotoAsync(string path = "", bool waitForNetworkIdle = true)
    {
        await Page.GotoAsync($"{BaseUrl}{path}");
        if (waitForNetworkIdle)
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task LoginAsAdminAsync()
    {
        var login = new LoginPage(Page);
        await login.LoginAsync(Data.TestData.AdminUsername, Data.TestData.AdminPassword);
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" })).ToBeVisibleAsync();
    }

    protected async Task WaitForAlertSuccessAsync(string expectedTextContains)
    {
        var alert = Page.Locator("div.alert-success");
        await Expect(alert).ToContainTextAsync(expectedTextContains, new() { Timeout = 15_000 });
    }

    // ← THIS IS THE CORRECT WAY TO GET Expect()
    protected ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
}