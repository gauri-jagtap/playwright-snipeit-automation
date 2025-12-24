using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SnipeIt.Playwright.Tests.Data;
using SnipeIt.Playwright.Tests.Models;
using SnipeIt.Playwright.Tests.Pages;

namespace SnipeIt.Playwright.Tests.Tests;

[TestFixture]
public class AssetLifecycleTests : BaseTest
{
    private AssetData _asset = null!;

    [SetUp]
    public void SetupData() => _asset = TestData.NewMacBookAsset();

    [Test]
    public async Task EndToEnd_Asset_Creation_With_Checkout_And_Validation()
    {
        Console.WriteLine("STARTING END-TO-END ASSET CREATION TEST");

        await new LoginPage(Page).LoginAsAdminAsync();
        Console.WriteLine("Logged in to Snipe-IT");

        var assetsPage = new AssetsPage(Page);
        await assetsPage.GotoCreateAsync();
        Console.WriteLine($"Creating new asset: {_asset.AssetTag}");

        
        var checkedOut = await assetsPage.CreateAssetAsync(_asset, checkoutToRandomUser: true);
        Console.WriteLine($"Asset {_asset.AssetTag} CREATED successfully!");

        if (checkedOut)
            Console.WriteLine($"Asset checked out to user during creation");
        else
            Console.WriteLine("No checkout performed (random user selection skipped)");


        Console.WriteLine($"Searching for asset {_asset.AssetTag} in hardware list...");
        await assetsPage.OpenAssetByTagAsync(_asset.AssetTag);
        Console.WriteLine($"Asset {_asset.AssetTag} opened successfully");

        var details = new AssetDetailsPage(Page);
        await details.ValidateBreadcrumbAsync(_asset.AssetTag, _asset.ModelName);
        Console.WriteLine("Breadcrumb validated");
        await details.ValidateSerialNumberAsync(_asset.SerialNumber);
        Console.WriteLine("Serial number validated");
        await details.ValidateHistoryHasCreationAsync();
        Console.WriteLine("History: Creation event found");

        if (checkedOut)
            await details.ValidateHistoryHasImmediateCheckoutAsync();
            Console.WriteLine("History: Immediate checkout event found");

        // Store a screenshot of history tab
        var historyTab = Page.Locator("#history");
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var screenshotPath = Path.Combine(projectRoot, $"history_checkout_{_asset.AssetTag}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
        await historyTab.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = screenshotPath
        });
        Console.WriteLine("Screenshot of History tab saved!");

        Console.WriteLine("ALL VALIDATIONS PASSED – ASSET FULLY VERIFIED");
    }
}