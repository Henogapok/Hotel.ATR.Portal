using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Hotel.ATR.Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        public IActionResult Index()
        {
            _httpContext.HttpContext.Response.Cookies.Append("iin", "880111300392");
            var data2 = _httpContext.HttpContext.Request.Cookies["iin"];

            HttpContext.Session.SetString("iin", "880111300392");
            var sessionData = HttpContext.Session.GetString("iin");

            /*CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Append("iin", "880111300392", options);*/

            string value = Request.Cookies["iin"];

            User user = new User();
            user.email = "ok@ok.com";
            _logger.LogError("У пользователя {email} возникла ошибка {errorMessage}", user.email, "Ошибка пользователя");


            Stopwatch sw = new Stopwatch();
            sw.Start();
            Thread.Sleep(1000);
            sw.Stop();
            _logger.LogInformation("Сервис отработал за {ElapsedMilliseconds}", sw.ElapsedMilliseconds);


            _logger.LogInformation("Logging Information");
            _logger.LogError("Logging Error");
            _logger.LogWarning("Logging Warning");
            _logger.LogDebug("Logging Debug");



            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(string login, string password, string ReturnUrl)
        {
            if (login == "admin" && password == "admin")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, login)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                Task.Delay(100).Wait();
                return Redirect(ReturnUrl);
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}