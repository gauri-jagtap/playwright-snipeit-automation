using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
namespace SnipeIt.Playwright.Tests.Pages;

public class ManufacturersPage : BasePage
{
    public ManufacturersPage(IPage page) : base(page) { }

    public async Task EnsureExistsAsync(string name)
    {
        await GotoAsync("/manufacturers");

        if (await Page.Locator($"text={name}").CountAsync() == 0)
        {
            await Page.ClickAsync("a:has-text('New')");
            await Page.FillAsync("input[name='name']", name);
            await Page.ClickAsync("button:has-text('Save')");
            await WaitForAlertSuccessAsync("Manufacturer created successfully");
        }
    }
}