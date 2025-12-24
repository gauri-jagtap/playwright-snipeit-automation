using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SnipeIt.Playwright.Tests.Pages;

namespace SnipeIt.Playwright.Tests.Tests;

[TestFixture]
public class PrerequisitesTests : BaseTest
{
    [Test, Order(1)]
    public async Task Ensure_Prerequisites()
    {
        await new LoginPage(Page).LoginAsAdminAsync();
        Console.WriteLine("Logged in");

        await new ManufacturersPage(Page).EnsureExistsAsync("Apple");
        Console.WriteLine("Manufacturer 'Apple' ready");

        await new CategoriesPage(Page).EnsureAssetCategoryExistsAsync("Laptops");
        Console.WriteLine("Category 'Laptops' ready");

        await new ModelsPage(Page).EnsureModelExistsAsync("MacBook Pro 13\"", "Apple", "Laptops");
        Console.WriteLine("Model 'MacBook Pro 13\"' ready");
        
        Console.WriteLine("ALL PREREQUISITES READY");
    }
}