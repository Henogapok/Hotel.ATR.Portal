using Hotel.ATR.Portal;
using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

string connectionString = "data source=178.89.186.221, 1434;initial catalog=aprelev_db;user id=aprelev_user;password=hH583z3i^;MultipleActiveResultSets=True;application name=EntityFramework;TrustServerCertificate=True";
builder.Services.AddDbContext<HotelAtrContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<APIEndpoint>(builder.Configuration.GetSection("APIEndpoint"));
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
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
//builder.Services.AddTransient<IRepository, Repository>();


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

/*builder.Services.AddCors(cors =>
{
    cors.AddPolicy("Policy_1", builder => builder
        .WithOrigins("http://localhost:5189/home/GetUser")
        .WithMethods("GET"));
});*/

var policyName = "Policy_1";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:62029/",
                                "http://localhost:5189/home/GetUser")
                .AllowCredentials();
        });
});

/*static void HandleMapOpen(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context
        .Response
        .WriteAsync("Hello");
    });
}*/



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

/*app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Before invoke app.Use\n");
    await next();
    await context.Response.WriteAsync("After invoke app.Use\n");

});

app.Map("/m1", HandleMapOpen);

app.Map("/m2", appMap =>
    {
        appMap.Run(async context =>
        {
            await context
            .Response
            .WriteAsync("Hello!");
        });
    }
);*/

app.UseRouting();

app.UseCors(policyName);

app.UseContentMiddleware();

var localOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localOptions.Value);


app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
