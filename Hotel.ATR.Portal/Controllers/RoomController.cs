using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        public IActionResult Index()
        {
            _logger.LogInformation("Logging Information");
            _logger.LogError("Logging Error");
            _logger.LogWarning("Logging Warning");
            _logger.LogDebug("Logging Debug");
            return View();
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
    }
}
