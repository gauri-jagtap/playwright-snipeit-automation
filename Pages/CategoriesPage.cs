using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
namespace SnipeIt.Playwright.Tests.Pages;

public class CategoriesPage : BasePage
{
    public CategoriesPage(IPage page) : base(page) { }

    public async Task EnsureAssetCategoryExistsAsync(string name)
    {
        await GotoAsync("/categories");

        if (await Page.Locator($"text={name}").CountAsync() == 0)
        {
            await Page.ClickAsync("a:has-text('New')");
            await Page.FillAsync("input[name='name']", name);
            await Page.CheckAsync("input[value='asset'][name='category_type']");
            await Page.ClickAsync("button:has-text('Save')");
            await WaitForAlertSuccessAsync("Category created successfully");
        }
    }
}