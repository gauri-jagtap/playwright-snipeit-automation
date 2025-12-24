using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
namespace SnipeIt.Playwright.Tests.Pages;

public class ModelsPage : BasePage
{
    public ModelsPage(IPage page) : base(page) { }

    public async Task EnsureModelExistsAsync(string modelName, string manufacturer, string category)
    {
        await GotoAsync("/hardware/models");

        if (await Page.Locator($"text={modelName}").CountAsync() == 0)
        {
            await Page.ClickAsync("a:has-text('New')");
            await Page.FillAsync("input[name='name']", modelName);
            await Page.SelectOptionAsync("select[name='manufacturer_id']", 
                new SelectOptionValue { Label = manufacturer });

            await Page.SelectOptionAsync("select[name='category_id']", 
                new SelectOptionValue { Label = category });
            await Page.ClickAsync("button:has-text('Save')");
            await WaitForAlertSuccessAsync("Model created successfully");
        }
    }
}