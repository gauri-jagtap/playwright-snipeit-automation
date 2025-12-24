using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SnipeIt.Playwright.Tests.Models;


namespace SnipeIt.Playwright.Tests.Data;

public static class TestData
{
    public static AssetData NewMacBookAsset()
    {
        var ts = DateTime.Now.ToString("HHmmss");
        return new AssetData
        {
            AssetTag = $"TESTMAC-{ts}",
            SerialNumber = $"TESTMAC-{ts}"
        };
    }

    public const string AdminUsername = "admin";
    public const string AdminPassword = "password";
    public const string BaseUrl = "https://demo.snipeitapp.com";
}