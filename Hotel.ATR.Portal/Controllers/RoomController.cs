using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Net.Http.Headers;
using System.Text;

namespace Hotel.ATR.Portal.Controllers
{
    public class RoomController : Controller
    {
        private IWebHostEnvironment webHost;
        private readonly ILogger<RoomController> _logger;

        public RoomController(IWebHostEnvironment webHost, ILogger<RoomController> _logger)
        {
            this.webHost = webHost;
            this._logger = _logger;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            RoomData rd = new RoomData();

            string jwt = GenerateJSONWebToken();

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                using (var response = await httpClient.GetAsync("http://localhost:5157/api/Room/"))
                {
                    string apiRequest = await response.Content.ReadAsStringAsync();

                    rd.Rooms = JsonConvert.DeserializeObject<List<Room>>(apiRequest);
                }
                using (var response = await httpClient.GetAsync("http://localhost:5157/api/Client/"))
                {
                    string apiRequest = await response.Content.ReadAsStringAsync();

                    rd.Clients = JsonConvert.DeserializeObject<List<Client>>(apiRequest);
                }
            }
           

            _logger.LogInformation("Logging Information");
            _logger.LogError("Logging Error");
            _logger.LogWarning("Logging Warning");
            _logger.LogDebug("Logging Debug");
            return View(rd);
        }
        public IActionResult RoomList()
        {
            return View();
        }
        public IActionResult RoomDetails()
        {
            return View();
        }

        /// <summary>
        ///1.string email
        ///2.User user
        ///3.Request.Form
        /// </summary>
        [HttpPost]
        public IActionResult SubscribeNewsLetter(IFormFile userFile)
        {
            var req = Request.Form["email"];

            string path = Path.Combine(webHost.WebRootPath, userFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                userFile.CopyTo(stream);
            }

            //return View("Index");
            return RedirectToAction("Index");
            //return View("~/Views/Home/Index.cshtml");
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4c53ce9de0ab7c9ce2f72f2b1447aa73"));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "John Doe",
                audience: "1516239022",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
