using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;


namespace SnipeIt.Playwright.Tests.Utils;

public static class Select2Helper
{
    public static async Task<bool> SelectRandomUserAsync(IPage page)
    {
        try
        {
            Console.WriteLine("Attempting to check out asset to a random user...");

            // Reliable way to click the Select2 dropdown
            await page.Locator("span.select2-selection__placeholder:has-text('Select a User')").ClickAsync();

            await page.WaitForTimeoutAsync(800);

            var options = page.Locator(".select2-results__option")
                .Filter(new() { HasNotText = "Select a User" })
                .Filter(new() { HasNotText = "Loading more results..." })
                .Filter(new() { HasNotText = "No results found" });

            var count = await options.CountAsync();
            if (count == 0)
            {
                Console.WriteLine("[CHECKOUT] No users available to select.");
                return false;
            }

            var index = new Random().Next(0, Math.Min(10, count));
            var user = await options.Nth(index).InnerTextAsync();
            await options.Nth(index).ClickAsync();

            Console.WriteLine($"[CHECKOUT] Asset checked out to: {user.Trim()}");

            await page.Keyboard.PressAsync("Escape");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CHECKOUT] Failed: {ex.Message}");
            await page.Keyboard.PressAsync("Escape");
            return false;
        }
    }
}