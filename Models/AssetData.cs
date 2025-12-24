using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
namespace SnipeIt.Playwright.Tests.Models;

public class AssetData
{
    public string AssetTag { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string ModelName { get; set; } = "MacBook Pro 13\"";
    public string ManufacturerName { get; set; } = "Apple";
    public string CategoryName { get; set; } = "Laptops";
    public string StatusLabel { get; set; } = "Ready to Deploy";
    public string? AssignedTo { get; set; }
}