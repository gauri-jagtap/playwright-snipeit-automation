using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SnipeIt.Playwright.Tests.Models;
using SnipeIt.Playwright.Tests.Utils;

namespace SnipeIt.Playwright.Tests.Pages;

public class AssetsPage : BasePage
{
    public AssetsPage(IPage page) : base(page) { }

    public async Task GotoCreateAsync() => await GotoAsync("/hardware/create");

    public async Task<bool> CreateAssetAsync(AssetData asset, bool checkoutToRandomUser = false)
    {
        Console.WriteLine($"Filling asset form → Tag: {asset.AssetTag}, Serial: {asset.SerialNumber}");

        await SelectModelAsync(asset.ModelName);

        await Page.Locator("input[name='asset_tags[1]']").FillAsync(asset.AssetTag);
        await Page.Locator("input[name='serials[1]']").FillAsync(asset.SerialNumber);
        await Page.SelectOptionAsync("select[name='status_id']", 
            new SelectOptionValue { Label = asset.StatusLabel });

        bool checkedOut = false;
        if (checkoutToRandomUser)
            checkedOut = await Select2Helper.SelectRandomUserAsync(Page);

        await Page.ClickAsync("button:has-text('Save')");
        Console.WriteLine("Submitted asset creation form...");

        var success = Page.Locator("div.alert-success")
            .Filter(new() { HasText = $"Asset with tag {asset.AssetTag} was created successfully" });

        await Expect(success).ToBeVisibleAsync(new() { Timeout = 20_000 });

        return checkedOut;
    }

    private async Task SelectModelAsync(string modelName)
    {
        await Page.ClickAsync("label:has-text('Model') + div .select2-selection");
        await Page.FillAsync(".select2-search__field", modelName);
        await Page.WaitForTimeoutAsync(800);
        await Page.ClickAsync($".select2-results__option:has-text('{modelName}')");
        await Page.Keyboard.PressAsync("Escape");
        await Task.Delay(600);
    }

    public async Task OpenAssetByTagAsync(string assetTag)
    {
        await GotoAsync("/hardware");
        await Utils.WaitUtils.WaitForTableLoadAsync(Page);

        var searchBox = Page.Locator("input.search-input");
        await searchBox.FillAsync(assetTag);
        await searchBox.PressAsync("Enter");

        await Utils.WaitUtils.WaitForTableLoadAsync(Page);

        var highlightedLink = Page.Locator($"a mark:has-text('{assetTag}')").First;
        await Expect(highlightedLink).ToBeVisibleAsync(new() { Timeout = 10_000 });
        await highlightedLink.ClickAsync();

        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}