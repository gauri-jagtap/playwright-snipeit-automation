using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
namespace SnipeIt.Playwright.Tests.Pages;

public class LoginPage : BasePage
{
    public LoginPage(IPage page) : base(page) { }

    private ILocator Username => Page.Locator("input[name='username']");
    private ILocator Password => Page.Locator("input[name='password']");
    private ILocator Submit => Page.Locator("button[type='submit']");

    public async Task LoginAsync(string username, string password)
    {
        await GotoAsync("/login");
        await Username.FillAsync(username);
        await Password.FillAsync(password);
        await Submit.ClickAsync();
    }
}