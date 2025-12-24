using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace SnipeIt.Playwright.Tests.Pages;

public class AssetDetailsPage : BasePage
{
    public AssetDetailsPage(IPage page) : base(page) { }

    public async Task ValidateBreadcrumbAsync(string expectedTag, string expectedModelContains)
    {
        Console.WriteLine("Validating breadcrumb contains correct asset tag and model...");

        var breadcrumb = Page.Locator("li.breadcrumb-item.active")
            .Filter(new() { HasText = $"#{expectedTag}" });

        await Expect(breadcrumb).ToBeVisibleAsync(new() { Timeout = 15_000 });

        var text = await breadcrumb.InnerTextAsync();
        var match = Regex.Match(text.Trim(), @"^#\s*([A-Z0-9-]+)\s*-\s*(.+)$");

        Assert.That(match.Success, Is.True, $"Breadcrumb format invalid: {text}");
        Assert.That(match.Groups[1].Value.Trim(), Is.EqualTo(expectedTag));
        Assert.That(match.Groups[2].Value.Trim(), Does.Contain(expectedModelContains).IgnoreCase);
    }

    public async Task ValidateSerialNumberAsync(string expectedSerial)
    {
        Console.WriteLine("Checking serial number...");
        var serialSpan = Page.Locator("span.js-copy-serial");
        await Expect(serialSpan).ToHaveTextAsync(expectedSerial, new() { Timeout = 10_000 });
    }

    public async Task ValidateHistoryHasCreationAsync()
    {
        Console.WriteLine("Checking History tab for 'create new' event...");
        await Page.GetByRole(AriaRole.Link, new() { Name = "History" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var row = Page.Locator("table#assetHistory tbody tr")
            .Filter(new() { HasText = "create new" });

        await Expect(row).ToBeVisibleAsync(new() { Timeout = 10_000 });
    }

    public async Task ValidateHistoryHasImmediateCheckoutAsync()
    {
        var row = Page.Locator("table#assetHistory tbody tr")
            .Filter(new() { HasText = "checkout" })
            .Filter(new() { HasText = "Checked out on asset creation" });

        await Expect(row).ToBeVisibleAsync(new() { Timeout = 10_000 });
    }
}