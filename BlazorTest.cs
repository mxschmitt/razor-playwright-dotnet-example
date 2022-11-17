using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp.Data;
using Microsoft.Playwright;

namespace PlaywrightTests;

[TestClass]
public class BlazorTest : PageTest
{
    private static WebApplication _app = null!;

    [AssemblyInitialize]
    public static void AssemblyInitialize2(TestContext context)
    {
        Console.Error.WriteLine("AssemblyInitialize");
        var baseDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions(){
            EnvironmentName = "Development",
            ContentRootPath = baseDir,
            WebRootPath = Path.Combine(baseDir, "wwwroot"),
            ApplicationName = "BlazorApp",
        });

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        _app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!_app.Environment.IsDevelopment())
        {
            _app.UseExceptionHandler("/Error");
        }


        _app.UseStaticFiles();

        _app.UseRouting();

        _app.MapBlazorHub();
        _app.MapFallbackToPage("/_Host");

        var readyTcs = new CancellationTokenSource();
        _ = Task.Run(async() =>
        {
            await _app.StartAsync(readyTcs.Token);
            readyTcs.Cancel();
        }).ConfigureAwait(false);
        readyTcs.Token.WaitHandle.WaitOne();
    }

    [AssemblyCleanup]
    public async static Task AssemblyCleanup()
    {
        await _app.StopAsync();
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        var options = base.ContextOptions() ?? new();
        options.BaseURL = "http://localhost:5000";
        return options;
    }
}