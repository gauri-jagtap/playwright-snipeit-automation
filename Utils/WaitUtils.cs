using Microsoft.Playwright;

namespace SnipeIt.Playwright.Tests.Utils;

public static class WaitUtils
{
    public static async Task WaitForTableLoadAsync(IPage page, double timeoutMs = 30000)
    {
        var spinner = page.Locator(".fixed-table-loading");

        if (await spinner.IsVisibleAsync())
        {
            await spinner.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
                Timeout = (float)timeoutMs
            });
        }
    }
}