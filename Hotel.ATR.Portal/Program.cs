using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
//builder.Host.ConfigureLogging(logging => {
//    logging.ClearProviders();
//    logging
//    //.AddDebug()
//    //.AddConsole()
//    //.AddEventLog()
//    .AddSeq();
//    }
//);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Home/login";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[]
    {
        new CultureInfo("ru-Ru"),
        new CultureInfo("kk-Kz"),
        new CultureInfo("en-Us")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "ru-Ru", uiCulture: "ru-Ru");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
//builder.Services.AddTransient<IRepository, Repository>();

string connectionString = builder.Configuration.GetConnectionString("DemoSeriLogDB");


    Log.Logger = new LoggerConfiguration()
        .WriteTo.Seq("http://localhost:5341/")
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" }, null, null, LogEventLevel.Error, null, null, null, null)
        .CreateLogger();


builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Domain = "localhost:62029";
    options.Cookie.Expiration = TimeSpan.FromSeconds(160);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.Name = "AspSessionName";
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

var localOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localOptions.Value);

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
