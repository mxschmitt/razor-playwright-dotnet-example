using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightTests;

[TestClass]
public class UnitTest1 : BlazorTest
{
    [TestMethod]
    public async Task EasyTest()
    {
        await Page.GotoAsync("/");

        await Expect(Page).ToHaveTitleAsync("Index");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Counter" }).ClickAsync();

        for (int i = 0; i < 10; i++)
        {
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync();
            await Expect(Page.GetByText($"Current count: {i + 1}")).ToBeVisibleAsync();
        }
    }
}